using MediatR;

namespace DOCSeal.Application.Orgs;

public record CreateRoleCommand(Guid OrgId, Guid RequesterId, string RoleName) : IRequest<string>;