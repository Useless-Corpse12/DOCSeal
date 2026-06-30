using MediatR;

namespace DOCSeal.Application.Usrs;

public record RegistrationSelfCommand(string Name, string Password, string Email,string Phone):IRequest<Guid>;