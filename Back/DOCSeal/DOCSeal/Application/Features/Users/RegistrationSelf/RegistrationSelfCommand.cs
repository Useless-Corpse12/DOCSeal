using MediatR;

namespace DOCSeal.Application.Features.Users.RegistrationSelf;

public record RegistrationSelfCommand(string Name, string Password, string Email):IRequest<Guid>;