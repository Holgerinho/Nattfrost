namespace NattfrostBackend.Services;

/// <summary>
/// Contract for sending frost alert emails.
/// Implement this interface to provide the actual email delivery logic.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Sends a frost alert notification to a subscriber.
    /// </summary>
    /// <param name="email">Subscriber's email address.</param>
    /// <param name="city">City where frost risk was detected.</param>
    Task SendFrostAlertAsync(string email, string city);
}
