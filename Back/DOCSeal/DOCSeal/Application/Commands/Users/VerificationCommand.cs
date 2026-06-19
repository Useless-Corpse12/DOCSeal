namespace DOCSeal.Application.Commands.Users;

public record VerificationCommand(Guid id, string verificationCode);