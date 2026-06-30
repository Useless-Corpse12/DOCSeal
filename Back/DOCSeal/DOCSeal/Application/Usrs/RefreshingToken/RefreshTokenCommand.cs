using MediatR;

namespace DOCSeal.Application.Usrs;

public record RefreshTokenCommand(string RefreshToken, string FingerPrint): IRequest<AuthResult>;