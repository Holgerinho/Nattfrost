namespace NattfrostBackend.Services;

/// <summary>
/// Default no-op implementation that logs frost alerts to the console.
/// Swap this out for a real email service when one is available.
/// </summary>
public class NoOpEmailService : IEmailNotificationService
{
    private readonly ILogger<NoOpEmailService> _logger;

    public NoOpEmailService(ILogger<NoOpEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendFrostAlertAsync(string email, string city)
    {
        _logger.LogInformation("[FROST ALERT] Send email to {Email} — frost risk in {City}", email, city);
        return Task.CompletedTask;
    }
}
