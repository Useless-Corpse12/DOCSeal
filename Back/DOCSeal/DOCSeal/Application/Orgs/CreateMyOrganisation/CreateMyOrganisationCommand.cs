using MediatR;

namespace DOCSeal.Application.Orgs;

public record CreateMyOrganisationCommand(Guid OwnerId, string Name):IRequest<Guid>;