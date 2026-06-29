using MediatR;

namespace DOCSeal.Application.Users.RefreshingToken;

public record RefreshTokenCommand(string RefreshToken, string FingerPrint): IRequest<AuthResult>;