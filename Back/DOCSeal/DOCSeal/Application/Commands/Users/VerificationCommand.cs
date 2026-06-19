namespace DOCSeal.Application.Commands.Users;

public record VerificationCommand(string login, string verificationCode);