using MediatR;

namespace DOCSeal.Application.Orgs;

public record UpdateRoleCommand(Guid OrgId, Guid RequesterId, string OldRoleName, string NewRoleName) : IRequest<string>;