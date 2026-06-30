using MediatR;

namespace DOCSeal.Application.Orgs;

public record InviteLinkGenerationCommand(Guid RequesterId, Guid OrgId, string Role, 
    bool IsOneTime, int DurationDays, string? TargetEmail ) : IRequest<string>;

