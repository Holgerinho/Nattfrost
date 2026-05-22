using System.Globalization;
using System.Text.Json;
using NattfrostBackend.Models;

namespace NattfrostBackend.Services;

/// <summary>
/// Thrown when the geocoding step fails — empty city name or API returns no results.
/// </summary>
public class GeocodingException : Exception
{
    public GeocodingException(string message) : base(message) { }
}

/// <summary>
/// Thrown when the forecast step fails — weather data missing or invalid.
/// </summary>
public class ForecastException : Exception
{
    public ForecastException(string message) : base(message) { }
}

/// <summary>
/// Calls the free Open-Meteo APIs for geocoding and weather forecasts.
/// Uses HttpClient injected via IHttpClientFactory for optimal connection pooling.
/// </summary>
public class OpenMeteoService
{
    private readonly HttpClient _client;

    public OpenMeteoService(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Looks up coordinates for a city using the Open-Meteo Geocoding API.
    /// Throws GeocodingException if the city is empty or not found.
    /// </summary>
    public async Task<LocationCoordinates> GetCoordinatesAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new GeocodingException("Geocoding failed: The city name must not be empty.");

        string encodedName = Uri.EscapeDataString(cityName);
        string url = $"https://geocoding-api.open-meteo.com/v1/search" +
                     $"?name={encodedName}" +
                     $"&count=1" +
                     $"&language=en" +
                     $"&format=json";

        HttpResponseMessage response = await _client.GetAsync(url);
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

    /// <summary>
    /// Checks whether a city exists in the Open-Meteo Geocoding API.
    /// Never throws — returns false for empty input, API errors, or no results.
    /// </summary>
    public async Task<bool> CityExistsAsync(string cityName)
    {
        try
        {
            await GetCoordinatesAsync(cityName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if any hourly temperature in the next 16 hours drops below 3°C.
    /// Returns true if frost risk exists, false otherwise.
    /// Throws ForecastException if the weather data is missing or invalid.
    /// </summary>
    public async Task<bool> HasFrostRiskAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast" +
                     $"?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
                     $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
                     $"&hourly=temperature_2m" +
                     $"&timezone=Europe/Stockholm" +
                     $"&forecast_days=2";

        HttpResponseMessage response = await _client.GetAsync(url);
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
}
