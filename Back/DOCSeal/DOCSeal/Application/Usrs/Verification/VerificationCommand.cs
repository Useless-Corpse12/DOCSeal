using MediatR;

namespace DOCSeal.Application.Usrs;

public record VerificationCommand(string Login, string VerificationCode): IRequest<Guid>;