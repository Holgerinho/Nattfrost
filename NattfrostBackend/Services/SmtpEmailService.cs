using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace NattfrostBackend.Services;

public class SmtpEmailService : IEmailNotificationService
{
    private readonly string _host;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _username;
    private readonly string _password;
    private readonly string _fromAddress;
    private readonly string _fromName;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        var section = configuration.GetSection("Email");

        _host = section["Host"] ?? throw new InvalidOperationException("Email:Host is required.");
        _port = int.TryParse(section["Port"], out var port) ? port : 587;
        _enableSsl = !bool.TryParse(section["EnableSsl"], out var enableSsl) || enableSsl;
        _username = section["Username"] ?? throw new InvalidOperationException("Email:Username is required.");
        _password = section["Password"] ?? throw new InvalidOperationException("Email:Password is required.");
        _fromAddress = section["FromAddress"] ?? throw new InvalidOperationException("Email:FromAddress is required.");
        _fromName = section["FromName"] ?? "Nattfrost";
        _logger = logger;
    }

    public async Task SendFrostAlertAsync(string email, string city)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_fromAddress, _fromName),
            Subject = $"Frost alert for {city}",
            Body = $"Hi!\n\nThere is a frost risk in {city} tonight.\n\n/Nattfrost",
            IsBodyHtml = false
        };

        message.To.Add(email);

        using var client = new SmtpClient(_host, _port)
        {
            EnableSsl = _enableSsl,
            Credentials = new NetworkCredential(_username, _password)
        };

        await client.SendMailAsync(message);
        _logger.LogInformation("Frost alert email sent to {Email} for {City}.", email, city);
    }
}
