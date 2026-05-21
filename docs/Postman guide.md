# Midnattsfrost — Using Postman with Open‑Meteo APIs

## Purpose

This document explains how to test the external APIs used in the Midnattsfrost project with Postman.

The goal is to:

- Understand the API responses
- Verify requests before writing backend code
- Make debugging easier
- Create reusable API tests for the team

---

# APIs Used

We will primarily use:

1. Open‑Meteo Geocoding API
2. Open‑Meteo Forecast API

Official documentation:

- https://open-meteo.com/
- https://open-meteo.com/en/docs/geocoding-api

---

# Install Postman

Download Postman:

https://www.postman.com/downloads/

Install and start the application.

---

# Create a Collection

Inside Postman:

1. Press "New"
2. Select "Collection"
3. Name it:

```text
Midnattsfrost APIs
```

Inside this collection we will create requests.

---

# Request 1 — Geocoding API

## Purpose

Convert a city or location name into coordinates.

Example:

```text
Gothenburg → latitude + longitude
```

---

## Create Request

Inside the collection:

1. Press "Add Request"
2. Name it:

```text
Geocoding Test
```

---

## Configure Request

Method:

```text
GET
```

URL:

```text
https://geocoding-api.open-meteo.com/v1/search?name=Gothenburg&count=1
```

Press:

```text
Send
```

---

# Example Response

You should receive JSON similar to this:

```json
{
  "results": [
    {
      "name": "Gothenburg",
      "latitude": 57.7072,
      "longitude": 11.9668,
      "country": "Sweden"
    }
  ]
}
```

---

# Important Fields

| Field | Description |
|---|---|
| name | Location name |
| latitude | Position latitude |
| longitude | Position longitude |
| country | Country |

These coordinates will later be used with the forecast API.

---

# Request 2 — Forecast API

## Purpose

Retrieve weather forecast data for coordinates.

---

## Create Request

Create another request:

```text
Forecast Test
```

Method:

```text
GET
```

URL:

```text
https://api.open-meteo.com/v1/forecast?latitude=57.7072&longitude=11.9668&hourly=temperature_2m
```

Press:

```text
Send
```

---

# Example Response

Example shortened response:

```json
{
  "hourly": {
    "time": [
      "2026-05-20T00:00",
      "2026-05-20T01:00"
    ],
    "temperature_2m": [
      2.4,
      -1.1
    ]
  }
}
```

---

# Frost Detection Logic

The backend can later analyze the temperatures.

Simple frost rule:

```text
If any temperature <= 0°C
→ Frost risk = true
```

Example:

| Time | Temperature |
|---|---|
| 00:00 | 2.4°C |
| 01:00 | -1.1°C |

Result:

```text
Frost risk detected
```

---

# Useful Query Parameters

## Geocoding API

| Parameter | Description |
|---|---|
| name | City or area |
| count | Number of results |

Example:

```text
?name=Stockholm&count=5
```

---

## Forecast API

| Parameter | Description |
|---|---|
| latitude | Latitude |
| longitude | Longitude |
| hourly | Hourly weather variables |

Example variables:

```text
temperature_2m
relative_humidity_2m
precipitation
wind_speed_10m
```

Example:

```text
hourly=temperature_2m,precipitation
```

---

# Recommended Team Workflow

Recommended workflow for Midnattsfrost:

```text
1. Test APIs in Postman
2. Verify JSON structure
3. Implement ASP.NET services
4. Connect backend endpoints
5. Connect React frontend
```

---

# Suggested Project API Tests

The team can later add these requests:

| Request | Purpose |
|---|---|
| Geocoding Test | Convert location → coordinates |
| Forecast Test | Get temperatures |
| POST Subscriber | Save subscriber |
| GET Subscribers | Retrieve subscribers |
| Frost Check Test | Check frost risk |

---

# Example Future Backend Endpoint

Example future endpoint:

```http
GET /api/weather/test?city=Gothenburg
```

Internal flow:

```text
1. Geocode city
2. Fetch forecast
3. Analyze temperatures
4. Return frost result
```

Possible response:

```json
{
  "city": "Gothenburg",
  "lowestTemperature": -2.1,
  "frostRisk": true
}
```

---

# Notes

- Open‑Meteo does not require an API key for basic usage
- Perfect for MVP and school projects
- Later the project can migrate to SMHI or another provider if needed

---

# End of Document
