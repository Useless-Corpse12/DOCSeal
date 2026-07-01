using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class InviteLinkGenerationHandler(
    AppDbContext dbContext, 
    IInviteCodeGenerator codeGenerator
) : IRequestHandler<InviteLinkGenerationCommand, string>
{
    public async Task<string> Handle(InviteLinkGenerationCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(up => up.UserId == cmd.RequesterId && up.OrgId == cmd.OrgId && up.PosName == "Big Boss", cnt);
        if (!isOwner) throw new Exception("Только владелец может создавать коды");

        if (cmd.Role == "Big Boss") throw new Exception("Нельзя создавать инвайты на роль Big Boss");
        
        var org = await dbContext.Organisations.FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt) 
                  ?? throw new Exception("Организация не найдена");
        
        if (!org.PossiblePositions.Contains(cmd.Role)) throw new Exception($"Роль '{cmd.Role}' не существует");

        var inviteCode = codeGenerator.GenerateToken(
            cmd.OrgId, 
            cmd.Role, 
            cmd.IsOneTime, 
            cmd.DurationDays
        );
        
        dbContext.OrganisationInviteCodes.Add(new Domain.Entities.OrganisationInviteCode(
            Guid.NewGuid(), 
            cmd.OrgId, 
            inviteCode
        ));
        
        await dbContext.SaveChangesAsync(cnt);

        return inviteCode;
    }
}