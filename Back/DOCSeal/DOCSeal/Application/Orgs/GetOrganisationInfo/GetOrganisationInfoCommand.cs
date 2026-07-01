using MediatR;

namespace DOCSeal.Application.Orgs;

public record GetOrganisationInfoCommand(Guid OrgId, Guid RequesterId) : IRequest<OrganisationInfoResponse>;

public record OrganisationInfoResponse(
    Guid Id,
    string Name,
    bool IsOwner,
    List<string> PossiblePositions,
    List<EmployeeInfo> Employees
);

public record EmployeeInfo(Guid UserId, string UserName, string Email, string Position);