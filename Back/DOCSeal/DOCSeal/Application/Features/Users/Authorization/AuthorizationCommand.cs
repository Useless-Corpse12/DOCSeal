using MediatR;

namespace DOCSeal.Application.Features.Users.Authorization;

public record AuthorizationCommand(string Login, string Password): IRequest<string>;