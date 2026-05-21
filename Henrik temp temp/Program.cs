using System.Globalization;
using System.Text.Json;

public class GeocodingException : Exception
{
    public GeocodingException(string message) : base(message) { }
}

public class ForecastException : Exception
{
    public ForecastException(string message) : base(message) { }
}

public record LocationCoordinates(string CityName, double Latitude, double Longitude);

class Program
{
    
    private static readonly HttpClient client = new HttpClient();

    static async Task<LocationCoordinates> GetCoordinatesAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new GeocodingException("Geocoding failed: The city name must not be empty.");

        string encodedName = Uri.EscapeDataString(cityName);
        string url = $"https://geocoding-api.open-meteo.com/v1/search?name={encodedName}&count=1&language=en&format=json";

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        if (!root.TryGetProperty("results", out JsonElement results) || results.GetArrayLength() == 0)
            throw new GeocodingException(
                $"Geocoding failed: Could not find coordinates for '{cityName}'. " +
                "Check the spelling and try again.");

        JsonElement first = results[0];
        string name = first.GetProperty("name").GetString() ?? cityName;
        double latitude = first.GetProperty("latitude").GetDouble();
        double longitude = first.GetProperty("longitude").GetDouble();

        return new LocationCoordinates(name, latitude, longitude);
    }

    static async Task<bool> HasFrostRiskAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast" +
                     $"?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
                     $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
                     $"&hourly=temperature_2m" +
                     $"&timezone=Europe/Stockholm" +
                     $"&forecast_days=2";

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        if (!root.TryGetProperty("hourly", out JsonElement hourly))
            throw new ForecastException("Forecast failed: Hourly weather data is missing from the response.");

        if (!hourly.TryGetProperty("temperature_2m", out JsonElement temperatures))
            throw new ForecastException("Forecast failed: Temperature data is missing from the hourly forecast.");

        int hoursToCheck = Math.Min(16, temperatures.GetArrayLength());

        for (int i = 0; i < hoursToCheck; i++)
        {
            double temp = temperatures[i].GetDouble();
            if (temp < 3.0)
                return true;
        }

        return false;
    }

    static async Task CheckCityForFrostAsync(string cityName)
    {
        LocationCoordinates location = await GetCoordinatesAsync(cityName);
        bool hasFrostRisk = await HasFrostRiskAsync(location.Latitude, location.Longitude);

        Console.WriteLine($"City:        {location.CityName}");
        Console.WriteLine($"Latitude:    {location.Latitude}");
        Console.WriteLine($"Longitude:   {location.Longitude}");
        Console.WriteLine($"Frost risk in the next 16 hours: {(hasFrostRisk ? "Yes" : "No")}");
    }

    static async Task Main(string[] args)
    {
        Console.WriteLine("Nattfrost - Frost risk checker (Ctrl+C or type 'quit' to exit)");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter a city name: ");
            string? cityName = Console.ReadLine();

            // Type quit or exit to quit
            if (string.Equals(cityName, "quit", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(cityName, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            // On empty input, loop again instead of crashing.
            if (string.IsNullOrWhiteSpace(cityName))
            {
                Console.WriteLine("Error: Please enter a city name.");
                Console.WriteLine();
                continue;
            }

            try
            {
                await CheckCityForFrostAsync(cityName);
            }
            catch (GeocodingException ex)
            {
                Console.WriteLine("Step failed: Geocoding");
                Console.WriteLine(ex.Message);
            }
            catch (ForecastException ex)
            {
                Console.WriteLine("Step failed: Forecast");
                Console.WriteLine(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error: Could not connect to the weather service.");
                Console.WriteLine($"Reason: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}
