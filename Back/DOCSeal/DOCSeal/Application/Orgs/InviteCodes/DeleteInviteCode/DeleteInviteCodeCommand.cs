using MediatR;

namespace DOCSeal.Application.Orgs;

public record DeleteInviteCodeCommand(Guid OrgId, Guid RequesterId, string Code) : IRequest<string>;