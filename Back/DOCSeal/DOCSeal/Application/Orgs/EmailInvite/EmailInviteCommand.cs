using MediatR;

namespace DOCSeal.Application.Orgs;

public record EmailInviteCommand(Guid RequesterId, string InviteCode, string TargetEmail ) : IRequest<string>;