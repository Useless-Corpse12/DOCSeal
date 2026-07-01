using MediatR;

namespace DOCSeal.Application.Orgs;
public record GetOrganisationsCommand(Guid RequesterId) : IRequest<GettingOrganisationsResponse>;
public record GettingOrganisationsResponse(List<OrgBrief> Orgs);
public record OrgBrief(Guid Id, string Name, bool IsOwner);