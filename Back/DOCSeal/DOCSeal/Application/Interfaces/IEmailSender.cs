namespace DOCSeal.Application.Interfaces;



public interface IEmailSender
{ 
    Task SendEmailAsync(string toEmail, string subject, string body); 
    Task SendRegistrationCodeAsync(string toEmail, string code);
}