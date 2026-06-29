using MediatR;

namespace DOCSeal.Application.Users.Verification;

public record VerificationCommand(string Login, string VerificationCode): IRequest<Guid>;