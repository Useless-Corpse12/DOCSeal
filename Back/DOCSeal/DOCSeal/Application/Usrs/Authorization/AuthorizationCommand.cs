using MediatR;

namespace DOCSeal.Application.Usrs;

public record AuthorizationCommand(string Login, string Password, string FingerPrint): IRequest<AuthResult>;