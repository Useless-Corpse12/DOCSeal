using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;


namespace DOCSeal.Infrastructure.Services.EmailService;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailSender> _logger;
    
    public EmailSender(IOptions<EmailSettings> settings, ILogger<EmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        try
        {
            await client.SendMailAsync(message);
            _logger.LogInformation(":Succes: Email sent to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ":Fail: Failed to send email to {Email}", toEmail);
            throw;
        }
    }

    public async Task SendRegistrationCodeAsync(string toEmail, string code)
    {
        var subject = "Код подтверждения DOCSeal";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Вас приветствует команда DOCSeal!</h2>
                <p>Ваш код подтверждения:</p>
                <h1 style='color: #2563eb; background: #f3f4f6; padding: 20px; text-align: center; border-radius: 8px;'>
                    {code}
                </h1>
                <p>Этот код действителен в течение 10 минут.</p>
                <p style='color: #6b7280; font-size: 12px;'>
                    Если вы не регистрировались в DOCSeal, просто проигнорируйте это письмо.
                </p>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }
}