using MediatR;

namespace DOCSeal.Application.Features.Users.Verification;

public record VerificationCommand(string Login, string VerificationCode): IRequest<Guid>;