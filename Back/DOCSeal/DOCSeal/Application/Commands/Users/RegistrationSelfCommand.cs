using MediatR;

namespace DOCSeal.Application.Commands.Users;

public record RegistrationSelfCommand(string Name, string Password, string Email):IRequest<Guid>;