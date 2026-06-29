using MediatR;

namespace DOCSeal.Application.Users.RegistrationSelf;

public record RegistrationSelfCommand(string Name, string Password, string Email,string Phone):IRequest<Guid>;