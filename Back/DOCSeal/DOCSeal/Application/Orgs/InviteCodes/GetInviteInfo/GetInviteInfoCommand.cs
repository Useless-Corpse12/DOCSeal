using MediatR;

namespace DOCSeal.Application.Orgs;

public record GetInviteInfoCommand(string Code) : IRequest<InviteInfoResponse>;

public record InviteInfoResponse(
    bool IsValid, 
    string OrgName, 
    string Role
);