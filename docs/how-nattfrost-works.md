# How Nattfrost Works — From Start to Finish

## What the program does

Nattfrost checks if a city has frost risk during the next 16 hours. You type a city name, it looks up the coordinates, pulls the weather forecast from Open-Meteo, and tells you whether any hourly temperature drops below 1°C.

No API key needed — Open-Meteo is free and open.

---

## Data types at the top of the file

Before any logic runs, three small building blocks are defined:

### `GeocodingException` (line 4)
A custom exception thrown when the geocoding step fails. This happens when you enter an empty city name or the API can't find the city.

### `ForecastException` (line 9)
A custom exception thrown when the forecast step fails. This happens when weather data is missing or unreadable.

### `LocationCoordinates` (line 14)
A **record** — a simple immutable data holder — that stores three values:

| Field       | Type     | Example        |
|-------------|----------|----------------|
| `CityName`  | `string` | `"Gothenburg"` |
| `Latitude`  | `double` | `57.70716`     |
| `Longitude` | `double` | `11.96679`     |

---

## Shared HttpClient (line 18)

```csharp
private static readonly HttpClient client = new HttpClient();
```

Created once as a static field and reused for every API call. Creating a new `HttpClient` per request would exhaust sockets over time — this is the correct pattern.

---

## The four methods — execution order

The program has four methods that call each other in a chain:

```
Main  ──►  CheckCityForFrostAsync  ──►  GetCoordinatesAsync   (Step 1)
                                   ──►  HasFrostRiskAsync      (Step 2)
```

### 1. `Main` (line 93) — Entry point

1. Asks the user for a city name with `Console.ReadLine()`.
2. If the input is empty, prints an error and returns.
3. Calls `CheckCityForFrostAsync(cityName)` inside a `try/catch` block.
4. The catch blocks are ordered from most-specific to least-specific:
   - `GeocodingException` — prints "Step failed: Geocoding"
   - `ForecastException` — prints "Step failed: Forecast"
   - `HttpRequestException` — prints "Could not connect to the weather service" + the reason
   - `Exception` — catches anything unexpected

---

### 2. `CheckCityForFrostAsync` (line 82) — Orchestrator

This method doesn't call any API itself. It just chains the two API-calling methods:

```
GetCoordinatesAsync(cityName)  →  gives us lat/lon
HasFrostRiskAsync(lat, lon)    →  gives us true/false
```

Then it prints all four pieces of information to the console.

---

### 3. `GetCoordinatesAsync` (line 20) — Step 1: Geocoding

Turns a city name into coordinates via the **Open-Meteo Geocoding API**.

#### What it does

1. **Validates input** — throws `GeocodingException` if the city name is empty or whitespace.

2. **Builds the URL** — a single line:
   ```csharp
   string url = $"https://geocoding-api.open-meteo.com/v1/search?name={encodedName}&count=1&language=en&format=json";
   ```
   - `name={encodedName}` — URL-encodes the city name so spaces and special characters work.
   - `count=1` — only need the best match.
   - `format=json` — returns JSON (also works without this, but explicit is clear).

3. **Sends the HTTP request** — `await client.GetAsync(url)` then `EnsureSuccessStatusCode()` which throws `HttpRequestException` on non-2xx responses.

4. **Parses the JSON response** using `System.Text.Json.JsonDocument`.

   The API returns this structure:
   ```json
   {
     "results": [
       {
         "name": "Gothenburg",
         "latitude": 57.70716,
         "longitude": 11.96679
       }
     ]
   }
   ```

5. **Checks for results** — if the `results` array is missing or empty, throws `GeocodingException`.

6. **Extracts values** from the first result and returns a `LocationCoordinates` record.

---

### 4. `HasFrostRiskAsync` (line 48) — Step 2: Forecast

Checks the **Open-Meteo Forecast API** for temperatures in the next 16 hours.

#### What it does

1. **Builds the URL**:
   ```csharp
   string url = $"https://api.open-meteo.com/v1/forecast" +
                $"?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
                $"?longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
                $"?hourly=temperature_2m" +
                $"?timezone=Europe/Stockholm" +
                $"?forecast_days=2";
   ```
   - `CultureInfo.InvariantCulture` ensures the decimal separator is a period (`.`), not a comma (`,`). Without this, a Swedish Windows machine would send `57,7` instead of `57.7`, which breaks the URL.
   - `hourly=temperature_2m` — requests hourly 2-metre air temperature.
   - `timezone=Europe/Stockholm` — returns times in Swedish local time.
   - `forecast_days=2` — requests 48 hours so there is always enough data for the 16-hour check.

2. **Sends the HTTP request** — same pattern as geocoding.

3. **Parses the JSON** — the API returns:
   ```json
   {
     "hourly": {
       "time": ["2026-05-20T00:00", "2026-05-20T01:00", ...],
       "temperature_2m": [7.8, 6.4, 3.1, 0.9, ...]
     }
   }
   ```

4. **Validates** that `hourly` and `temperature_2m` exist — throws `ForecastException` if not.

5. **Checks the first 16 hours** — loops through up to 16 temperature values. If **any** value is below `1.0°C`, returns `true` immediately. If all 16 are ≥ 1°C, returns `false`.

   > The threshold is `< 1.0` (strictly less than), so 1.0°C is considered safe.

---

## Error handling flow

Here's what happens in each failure scenario:

| Scenario                           | Exception thrown       | Caught by                     | User sees                                           |
|------------------------------------|------------------------|-------------------------------|-----------------------------------------------------|
| Empty city name                    | `GeocodingException`   | `catch (GeocodingException)`  | "Step failed: Geocoding" + explanation              |
| City not found by API              | `GeocodingException`   | `catch (GeocodingException)`  | "Step failed: Geocoding" + explanation              |
| Missing forecast data              | `ForecastException`    | `catch (ForecastException)`   | "Step failed: Forecast" + explanation               |
| No internet / API down / timeout   | `HttpRequestException` | `catch (HttpRequestException)`| "Could not connect" + HTTP reason                   |
| Anything else (bug)                | `Exception`            | `catch (Exception)`           | "An unexpected error occurred" + message            |

---

## How to run it

From the project folder:

```
cd Nattfrost
dotnet run
```

Type a city name when prompted. Examples:

```
Enter a city name: Gothenburg
City:        Gothenburg
Latitude:    57,70716
Longitude:   11,96679
Frost risk in the next 16 hours: No
```

```
Enter a city name: asdfxyzqwerty
Step failed: Geocoding
Geocoding failed: Could not find coordinates for 'asdfxyzqwerty'. Check the spelling and try again.
```

---

## Key concepts used

| Concept               | Where                                   |
|-----------------------|-----------------------------------------|
| `async` / `await`     | All API-calling methods                 |
| `HttpClient`          | Shared static field (line 18)           |
| `System.Text.Json`    | JSON parsing with `JsonDocument`        |
| Custom exceptions     | `GeocodingException`, `ForecastException` |
| `record`              | `LocationCoordinates` (line 14)         |
| `CultureInfo.InvariantCulture` | Forecast URL decimal formatting |
| `Uri.EscapeDataString` | Safe URL encoding of city name          |

---

## The Open-Meteo APIs used

| Purpose    | Endpoint                                                |
|------------|---------------------------------------------------------|
| Geocoding  | `https://geocoding-api.open-meteo.com/v1/search`        |
| Forecast   | `https://api.open-meteo.com/v1/forecast`                |

Both are free, require no API key, and return JSON.
