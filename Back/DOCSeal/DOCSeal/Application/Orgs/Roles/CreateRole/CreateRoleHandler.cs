using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class CreateRoleHandler(AppDbContext dbContext) : IRequestHandler<CreateRoleCommand, string>
{
    public async Task<string> Handle(CreateRoleCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId && p.PosName == "Big Boss", cnt);

        if (!isOwner) throw new Exception("Только владелец может управлять ролями");

        var org = await dbContext.Organisations
                      .FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt)
                  ?? throw new Exception("Организация не найдена");

        if (org.PossiblePositions.Contains(cmd.RoleName)) throw new Exception("Роль с таким названием уже существует");

        org.PossiblePositions.Add(cmd.RoleName);
        await dbContext.SaveChangesAsync(cnt);

        return $"Роль {cmd.RoleName} создана";
    }
}