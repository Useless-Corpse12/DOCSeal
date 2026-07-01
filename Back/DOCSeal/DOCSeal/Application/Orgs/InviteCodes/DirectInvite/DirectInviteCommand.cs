using MediatR;

namespace DOCSeal.Application.Orgs;

public record DirectInviteCommand(
    Guid RequesterId, 
    Guid OrgId, 
    string Role, 
    int DurationDays, 
    string TargetEmail
) : IRequest<string>;