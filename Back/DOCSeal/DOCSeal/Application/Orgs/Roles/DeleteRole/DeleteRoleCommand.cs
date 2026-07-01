using MediatR;

namespace DOCSeal.Application.Orgs;

public record DeleteRoleCommand(Guid OrgId, Guid RequesterId, string RoleName) : IRequest<string>;