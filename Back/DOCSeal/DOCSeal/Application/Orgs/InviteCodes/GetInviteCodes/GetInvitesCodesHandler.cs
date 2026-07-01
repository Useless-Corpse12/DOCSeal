using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class GetInviteCodesHandler(
    AppDbContext dbContext,
    IInviteCodeGenerator codeGenerator
) : IRequestHandler<GetInviteCodesCommand, List<InviteCodeInfo>>
{
    public async Task<List<InviteCodeInfo>> Handle(GetInviteCodesCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId && p.PosName == "Big Boss", cnt);

        if (!isOwner) throw new Exception("Только владелец может просматривать коды");

        var codes = await dbContext.OrganisationInviteCodes
            .Where(c => c.OrganisationId == cmd.OrgId)
            .ToListAsync(cnt);

        return codes.Select(c => {
            var payload = codeGenerator.ValidateToken(c.InviteCode);
            return new InviteCodeInfo(
                c.InviteCode,
                payload.Role,
                payload.IsOneTime,
                payload.DurationDays,
                DateTime.UtcNow
            );
        }).ToList();
    }
}