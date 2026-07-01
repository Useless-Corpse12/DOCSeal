using MediatR;

namespace DOCSeal.Application.Orgs;

public record AcceptInviteCommand(
    Guid UserId, 
    string Code
) : IRequest<string>;