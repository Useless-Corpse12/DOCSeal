using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class UpdateRoleHandler(AppDbContext dbContext) : IRequestHandler<UpdateRoleCommand, string>
{
    public async Task<string> Handle(UpdateRoleCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId && p.PosName == "Big Boss", cnt);

        if (!isOwner) throw new Exception("Только владелец может управлять ролями");

        if (cmd.OldRoleName == "Big Boss") throw new Exception("Нельзя изменить системную роль Big Boss");

        var org = await dbContext.Organisations
                      .FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt)
                  ?? throw new Exception("Организация не найдена");

        if (!org.PossiblePositions.Contains(cmd.OldRoleName)) throw new Exception("Старая роль не найдена");
        if (org.PossiblePositions.Contains(cmd.NewRoleName)) throw new Exception("Роль с таким названием уже существует");

        var index = org.PossiblePositions.IndexOf(cmd.OldRoleName);
        org.PossiblePositions[index] = cmd.NewRoleName;

        var affectedUsers = await dbContext.UserPositions
            .Where(p => p.OrgId == cmd.OrgId && p.PosName == cmd.OldRoleName)
            .ToListAsync(cnt);

        foreach (var user in affectedUsers)
        {
            user.PosName = cmd.NewRoleName;
        }

        await dbContext.SaveChangesAsync(cnt);

        return $"Роль переименована. Обновлено {affectedUsers.Count} сотрудников";
    }
}