# Midnattsfrost — 2-Day Setup Plan and Detailed Project Guide

## Project Idea

**Midnattsfrost** is a local web application built with:

- ASP.NET Web API backend
- React frontend
- SQLite or SQL Server database
- Open-Meteo Geocoding API
- Open-Meteo Forecast API

The user enters:

```text
Email
City / location
```

The app saves the user in the database and later checks if frost is expected during the night or early morning.

If frost is expected, the user should eventually receive a warning.

---

# Main 2-Day Goal

## Day 1

```text
Lock the data layer
Create project skeleton
Build first vertical slice
```

## Day 2

```text
Expand vertical slice
Add geocoding
Add weather forecast
Add frost risk logic
Prepare background frost check
```

---

# Core Development Strategy

## Lock Data Layer Early

This means deciding early how the database should look.

For the MVP, the most important database entity is:

```text
Subscriber
```

Suggested fields:

```text
Id
Email
LocationName
Latitude
Longitude
CreatedAt
IsActive
```

The point of locking the data layer early is to avoid chaos later.

Once this model is agreed on, the team can build backend, frontend, and API logic around the same structure.

---

## Build a Vertical Slice

A vertical slice means building one complete feature through all layers of the app.

For Midnattsfrost, the first vertical slice should be:

```text
React form
↓
ASP.NET API endpoint
↓
Service layer
↓
Database
↓
Response back to frontend
```

The first goal is not to build everything.

The first goal is to prove that the whole chain works.

---

# Recommended Folder Structure

```text
midnattsfrost/
│
├── backend/
│   └── Midnattsfrost.Api/
│
├── frontend/
│   └── midnattsfrost-client/
│
├── docs/
│   ├── mockup.md
│   ├── api-plan.md
│   ├── database-plan.md
│   └── postman-guide.md
│
└── README.md
```

---

# Day 1 — Detailed Plan

## Step 1 — Create Git Repository

Create a GitHub repository called:

```text
midnattsfrost
```

Clone it locally:

```bash
git clone <your-repo-url>
cd midnattsfrost
```

Create base folders:

```bash
mkdir backend
mkdir frontend
mkdir docs
```

Commit the first structure:

```bash
git add .
git commit -m "Create initial project structure"
```

---

## Step 2 — Create a Simple Mockup

Before coding too much, agree on the basic UI.

Create:

```text
docs/mockup.md
```

Suggested first UI:

```text
Midnattsfrost

Get warned before frost hits your area.

[ Email input    ]
[ Location input ]

[ Register me ]

Result:
"You are registered. We will check frost risk every evening."
```

This keeps the MVP small and clear.

---

## Step 3 — Decide the Data Layer

Create:

```text
docs/database-plan.md
```

Write the locked first entity:

```text
Subscriber
- Id
- Email
- LocationName
- Latitude
- Longitude
- CreatedAt
- IsActive
```

Recommended for school MVP:

```text
SQLite + Entity Framework Core
```

Why SQLite?

- Easy local setup
- No separate server needed
- Good for small local projects
- Easy for all team members to run

Alternative:

```text
SQL Server / LocalDB
```

This is also good, but may create more setup problems for the team.

---

## Step 4 — Create ASP.NET Backend

Go into backend folder:

```bash
cd backend
dotnet new webapi -n Midnattsfrost.Api
cd Midnattsfrost.Api
```

Run the backend:

```bash
dotnet run
```

Confirm that Swagger opens or that the API starts successfully.

Suggested backend folders:

```text
Controllers/
Data/
Models/
Services/
Dtos/
Exceptions/
```

---

## Step 5 — Add Entity Framework Core

For SQLite:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Install EF tool if needed:

```bash
dotnet tool install --global dotnet-ef
```

Create the first database model:

```text
Models/Subscriber.cs
```

Suggested fields:

```text
Id
Email
LocationName
Latitude
Longitude
CreatedAt
IsActive
```

Create database context:

```text
Data/AppDbContext.cs
```

Register it in the backend startup configuration.

Then create first migration:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

The goal is to prove that the database works early.

---

## Step 6 — Create First Backend Endpoint

Create a controller:

```text
Controllers/SubscribersController.cs
```

First endpoints:

```http
POST /api/subscribers
GET /api/subscribers
```

For Day 1, the `POST` endpoint can save:

```text
Email
LocationName
```

Latitude and longitude may be left empty/null at first, or temporarily set to 0.

The most important thing is proving:

```text
Request reaches backend
Data is saved
Response is returned
```

---

## Step 7 — Test Backend with Postman

Create a Postman collection:

```text
Midnattsfrost APIs
```

Add request:

```http
POST http://localhost:<port>/api/subscribers
```

Example body:

```json
{
  "email": "test@example.com",
  "locationName": "Gothenburg"
}
```

Expected result:

```json
{
  "message": "Subscriber saved successfully"
}
```

Then test:

```http
GET http://localhost:<port>/api/subscribers
```

Expected result:

```json
[
  {
    "id": 1,
    "email": "test@example.com",
    "locationName": "Gothenburg"
  }
]
```

---

## Step 8 — Create React Frontend

Go back to project root:

```bash
cd ../../frontend
```

Create React app with Vite:

```bash
npm create vite@latest midnattsfrost-client
cd midnattsfrost-client
npm install
npm run dev
```

Create a simple form with:

```text
Email input
Location input
Submit button
Result message
```

The frontend should send a request to:

```http
POST /api/subscribers
```

---

## Step 9 — Complete First Vertical Slice

The Day 1 vertical slice is complete when this works:

```text
User enters email + location in React
↓
Frontend sends POST request
↓
ASP.NET receives request
↓
Subscriber is saved in database
↓
Backend returns success
↓
React shows confirmation message
```

End of Day 1 target:

```text
The app can save a subscriber from frontend to database.
```

---

# Day 2 — Detailed Plan

## Step 1 — Test Open-Meteo APIs in Postman

Before writing C# code, test external APIs manually.

## Geocoding API Test

Purpose:

```text
Convert city name to latitude and longitude
```

Postman request:

```http
GET https://geocoding-api.open-meteo.com/v1/search?name=Gothenburg&count=1&language=en&format=json
```

Expected useful fields:

```text
name
latitude
longitude
country
```

Example result:

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

## Forecast API Test

Purpose:

```text
Get upcoming hourly temperatures
```

Postman request:

```http
GET https://api.open-meteo.com/v1/forecast?latitude=57.7072&longitude=11.9668&hourly=temperature_2m&timezone=Europe/Stockholm&forecast_days=2
```

Expected useful fields:

```text
hourly.time
hourly.temperature_2m
```

Example simplified result:

```json
{
  "hourly": {
    "time": [
      "2026-05-20T19:00",
      "2026-05-20T20:00",
      "2026-05-21T02:00"
    ],
    "temperature_2m": [
      4.2,
      2.1,
      -0.8
    ]
  }
}
```

Manual frost logic:

```text
If any temperature in the next 16 hours is below 1°C
→ frost risk = true
```

---

## Step 2 — Add Geocoding Service

Create service:

```text
Services/GeocodingService.cs
```

Responsibility:

```text
Input: city name
Output: latitude + longitude
```

Suggested method:

```text
GetCoordinatesAsync(string cityName)
```

It should:

```text
Call Open-Meteo Geocoding API
Check if result exists
Return coordinates
Throw exception if city is invalid or no result exists
```

---

## Step 3 — Add Weather Service

Create service:

```text
Services/WeatherService.cs
```

Responsibility:

```text
Input: latitude + longitude
Output: forecast temperatures
```

Suggested method:

```text
HasFrostRiskAsync(double latitude, double longitude)
```

It should:

```text
Call Open-Meteo Forecast API
Read hourly temperatures
Check the upcoming 16 hours
Return true if any temperature < 1°C
Return false otherwise
Throw exception if forecast data is missing
```

---

## Step 4 — Add Frost Check Service

Create service:

```text
Services/FrostCheckService.cs
```

Responsibility:

```text
Coordinate the full frost check
```

Suggested method:

```text
CheckCityForFrostAsync(string cityName)
```

Flow:

```text
City name
↓
GeocodingService
↓
Coordinates
↓
WeatherService
↓
Frost risk result
```

---

## Step 5 — Create Test Endpoint

Create endpoint:

```http
GET /api/weather/test?city=Gothenburg
```

Expected response:

```json
{
  "city": "Gothenburg",
  "latitude": 57.7072,
  "longitude": 11.9668,
  "frostRisk": true,
  "message": "Frost risk detected during the upcoming 16 hours."
}
```

This is a very good Day 2 vertical slice because it proves:

```text
Your backend can use external APIs
Your backend can process weather data
Your backend can return business logic result
```

---

## Step 6 — Update Subscriber Registration Flow

Once geocoding works, improve:

```http
POST /api/subscribers
```

Instead of only saving city name, it should now:

```text
Receive email + city
↓
Geocode city
↓
Save email + city + latitude + longitude
```

Updated saved subscriber:

```text
Email: test@example.com
LocationName: Gothenburg
Latitude: 57.7072
Longitude: 11.9668
IsActive: true
```

---

## Step 7 — Prepare Background Frost Check

The full background job does not need to be finished during the MVP, but the team can prepare it.

Options:

```text
ASP.NET BackgroundService
Hangfire
Quartz.NET
Manual endpoint for demo
```

For school MVP, recommended:

```text
Manual endpoint first
BackgroundService later
```

Example manual demo endpoint:

```http
POST /api/frost-check/run
```

This endpoint can:

```text
Get all active subscribers
Check weather for each location
Return frost results
```

Later it can be replaced by automatic daily checking at 18:00 or 19:00.

---

# Recommended Frost Logic

At 18:00 or 19:00, the app should check:

```text
Upcoming 16 hours
```

Reason:

```text
19:00 + 16 hours = 11:00 next day
```

This covers:

```text
Evening
Night
Early morning
```

Simple frost rule:

```text
If any forecasted temperature < 1°C
→ frost warning
```

Why less than 1°C instead of 0°C?

```text
Forecasts are not perfect
Ground temperature can be colder than air temperature
A small safety margin is useful for plants
```

---

# Recommended Team Workflow

During mob programming, use one driver and rotate.

Suggested roles:

```text
Driver: writes code
Navigator: guides the next step
Researcher: checks docs/API responses
Tester: tests in Postman/browser
```

Rotate every:

```text
15–20 minutes
```

---

# Suggested Commit Plan

## Day 1 Commits

```text
Create initial project structure
Add backend web API project
Add frontend React project
Add subscriber entity and database context
Add initial EF migration
Add subscriber API endpoints
Add basic React registration form
Connect frontend form to backend API
```

## Day 2 Commits

```text
Add Open-Meteo geocoding service
Add Open-Meteo weather service
Add frost check service
Add weather test endpoint
Update subscriber registration with coordinates
Add manual frost check endpoint
Add Postman documentation
```

---

# Final MVP Checklist

## Backend

```text
ASP.NET Web API runs
Database works
Subscriber can be saved
Geocoding works
Forecast API works
Frost risk logic works
Errors are handled clearly
```

## Frontend

```text
User can enter email
User can enter city
User can submit form
User gets success or error message
```

## Postman

```text
Geocoding API tested
Forecast API tested
POST subscriber tested
GET subscribers tested
Weather test endpoint tested
```

## Demo Flow

The final demo should show:

```text
1. Open React app
2. Enter email + city
3. Save subscriber
4. Test frost risk for city
5. Show result from backend
```

---

# Recommended MVP Scope

Include:

```text
Register subscriber
Save to database
Geocode city
Check forecast
Detect frost risk
Show result
```

Do not include yet:

```text
Login
Admin dashboard
Real email sending
Advanced weather analysis
Maps
User accounts
```

These can be future features.

---

# Future Improvements

After the MVP works, possible improvements are:

```text
Send real email warnings
Run automatic daily check at 18:00 or 19:00
Add unsubscribe link
Add admin page
Add frost history
Add severity levels
Use SMHI for Swedish weather data
Add more weather variables like humidity and wind speed
```

---

# Summary

The best strategy is:

```text
Day 1:
Lock database structure
Create skeleton
Build first vertical slice from React to database

Day 2:
Add Open-Meteo geocoding
Add weather forecast
Add frost risk logic
Prepare background checking
```

This gives the team a realistic, testable, and professional MVP.
