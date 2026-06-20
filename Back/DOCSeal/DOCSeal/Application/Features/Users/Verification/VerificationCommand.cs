namespace DOCSeal.Application.Features.Users.Verification;

public record VerificationCommand(string Login, string VerificationCode);