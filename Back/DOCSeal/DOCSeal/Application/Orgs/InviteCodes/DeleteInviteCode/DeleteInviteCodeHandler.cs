using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class DeleteInviteCodeHandler(AppDbContext dbContext) : IRequestHandler<DeleteInviteCodeCommand, string>
{
    public async Task<string> Handle(DeleteInviteCodeCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId && p.PosName == "Big Boss", cnt);

        if (!isOwner) throw new Exception("Только владелец может удалять коды");

        var invite = await dbContext.OrganisationInviteCodes
                         .FirstOrDefaultAsync(i => i.InviteCode == cmd.Code && i.OrganisationId == cmd.OrgId, cnt)
                     ?? throw new Exception("Код не найден");

        dbContext.OrganisationInviteCodes.Remove(invite);
        await dbContext.SaveChangesAsync(cnt);

        return "Код приглашения удалён";
    }
}