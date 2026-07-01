using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class DeleteRoleHandler(AppDbContext dbContext) : IRequestHandler<DeleteRoleCommand, string>
{
    public async Task<string> Handle(DeleteRoleCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId && p.PosName == "Big Boss", cnt);

        if (!isOwner) throw new Exception("Только владелец может управлять ролями");

        if (cmd.RoleName == "Big Boss") throw new Exception("Нельзя удалить системную роль Big Boss");

        var org = await dbContext.Organisations
                      .FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt)
                  ?? throw new Exception("Организация не найдена");

        if (!org.PossiblePositions.Contains(cmd.RoleName)) throw new Exception("Роль не найдена");

        var otherRoles = org.PossiblePositions.Where(r => r != "Big Boss" && r != cmd.RoleName).ToList();
        if (!otherRoles.Any()) throw new Exception("Нельзя удалить последнюю роль. Должна остаться хотя бы одна кроме Big Boss");

        var targetRole = otherRoles.First();

        var affectedUsers = await dbContext.UserPositions
            .Where(p => p.OrgId == cmd.OrgId && p.PosName == cmd.RoleName)
            .ToListAsync(cnt);

        foreach (var user in affectedUsers)
        {
            user.PosName = targetRole;
        }

        org.PossiblePositions.Remove(cmd.RoleName);
        await dbContext.SaveChangesAsync(cnt);

        return $"Роль удалена. {affectedUsers.Count} сотрудников переведены на роль {targetRole}";
    }
}