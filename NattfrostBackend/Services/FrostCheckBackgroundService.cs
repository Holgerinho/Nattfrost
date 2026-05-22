using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NattfrostBackend.Data;
//using Nattfrost.Entities;
//using Microsoft.AspNetCore.OpenApi;

namespace NattfrostBackend.Services;

/// <summary>
/// Background service that runs once per day at 19:00.
/// Loops through all subscribers, checks their cities for frost risk,
/// and triggers email alerts via IEmailNotificationService.
/// </summary>
public class FrostCheckBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FrostCheckBackgroundService> _logger;

    // Tracks the last date the check ran, to avoid running more than once per day.
    private DateOnly _lastRunDate = DateOnly.MinValue;

    public FrostCheckBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<FrostCheckBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FrostCheckBackgroundService started. Will run daily at 19:00.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;

            // Trigger when the hour is 19 and we haven't run today yet.
            if (now.Hour == 19 && _lastRunDate != DateOnly.FromDateTime(now))
            {
                _logger.LogInformation("19:00 reached — starting daily frost check.");
                _lastRunDate = DateOnly.FromDateTime(now);

                try
                {
                    await RunFrostCheckAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled error during daily frost check.");
                }
            }

            // Check again in 30 seconds.                                
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    /// <summary>
    /// Fetches all subscribers, checks each city for frost risk,
    /// and sends alerts for any that are at risk.
    /// </summary>
    public async Task RunFrostCheckAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var openMeteo = scope.ServiceProvider.GetRequiredService<OpenMeteoService>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();

        var subscribers = await db.Subscribers.ToListAsync(stoppingToken);

        _logger.LogInformation("Checking {Count} subscriber(s) for frost risk.", subscribers.Count);

        foreach (var subscriber in subscribers)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            try
            {
                // Step 1: Does the city exist? 
                // kanske ta bort, lite onödigt med extra koll !!!
                if (!await openMeteo.CityExistsAsync(subscriber.City))
                {
                    _logger.LogWarning("Skipping {Email} — city '{City}' not found in geocoding API.",
                        subscriber.Email, subscriber.City);
                    continue;
                }

                // Step 2: Get coordinates.
                var coords = await openMeteo.GetCoordinatesAsync(subscriber.City);

                // Step 3: Check frost risk.
                if (await openMeteo.HasFrostRiskAsync(coords.Latitude, coords.Longitude))
                {
                    // Step 4: Frost risk detected — send alert.
                    await emailService.SendFrostAlertAsync(subscriber.Email, subscriber.City);
                    _logger.LogInformation("Frost alert sent to {Email} for {City}.",
                        subscriber.Email, subscriber.City);
                }
                else
                {
                    _logger.LogInformation("No frost risk for {Email} in {City}.",
                        subscriber.Email, subscriber.City);
                }
            }
            catch (GeocodingException ex)
            {
                _logger.LogWarning(ex, "Geocoding failed for {Email} ({City}).",
                    subscriber.Email, subscriber.City);
            }
            catch (ForecastException ex)
            {
                _logger.LogError(ex, "Forecast failed for {Email} ({City}).",
                    subscriber.Email, subscriber.City);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error for {Email} ({City}).",
                    subscriber.Email, subscriber.City);
            }
        }

        _logger.LogInformation("Daily frost check complete.");
    }
}
