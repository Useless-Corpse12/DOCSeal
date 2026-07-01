using MediatR;

namespace DOCSeal.Application.Orgs;

public record GetInviteCodesCommand(Guid OrgId, Guid RequesterId) : IRequest<List<InviteCodeInfo>>;

public record InviteCodeInfo(string Code, string Role, bool IsOneTime, int DurationDays, DateTime CreatedAt );