using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Usrs.DeleteMyAccount;

public class DeleteMyAccountHandler(AppDbContext dbContext):IRequestHandler<DeleteMyAccountCommand>
{
    public async Task Handle(DeleteMyAccountCommand cmd, CancellationToken cnt)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x=>x.Id == cmd.Id,cnt) 
                        ?? throw new Exception("User not found");
        
        await dbContext.RefreshTokens
            .Where(t => t.UserId == cmd.Id)
            .ExecuteDeleteAsync(cnt);
        
        var ownedOrgIds = await dbContext.UserPositions
            .Where(up => up.UserId == cmd.Id && up.PosName == "Big Boss")
            .Select(up => up.OrgId)
            .ToListAsync(cnt);
        
        if (ownedOrgIds.Any())
        {
            await dbContext.UserPositions
                .Where(up => ownedOrgIds.Contains(up.OrgId))
                .ExecuteDeleteAsync(cnt);
            
            await dbContext.Organisations
                .Where(o => ownedOrgIds.Contains(o.Id))
                .ExecuteDeleteAsync(cnt);
        }

        await dbContext.UserPositions
            .Where(up => up.UserId == cmd.Id)
            .ExecuteDeleteAsync(cnt);
        
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cnt);
    }
    
}