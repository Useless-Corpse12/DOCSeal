using MediatR;

namespace DOCSeal.Application.Users.Authorization;

public record AuthorizationCommand(string Login, string Password, string FingerPrint): IRequest<AuthResult>;