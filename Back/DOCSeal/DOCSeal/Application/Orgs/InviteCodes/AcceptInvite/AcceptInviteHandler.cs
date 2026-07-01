using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class AcceptInviteHandler(
    AppDbContext dbContext,
    IInviteCodeGenerator codeGenerator
) : IRequestHandler<AcceptInviteCommand, string>
{
    public async Task<string> Handle(AcceptInviteCommand cmd, CancellationToken cnt)
    {
        var payload = codeGenerator.ValidateToken(cmd.Code);
        
        var invite = await dbContext.OrganisationInviteCodes.FirstOrDefaultAsync
                         (i => i.InviteCode == cmd.Code, cnt)
                     ?? throw new Exception("Инвайт уже использован");

        var inviteOrg = await dbContext.Organisations.FirstOrDefaultAsync
            (or => or.Id == payload.OrgId, cnt) 
            ?? throw new Exception("Организация не найдена");

        if (!inviteOrg.PossiblePositions.Contains(payload.Role))
            throw new Exception($"Роль '{payload.Role}' не существует в организации");

        var alreadyInOrg = await dbContext.UserPositions
            .AnyAsync(up => up.UserId == cmd.UserId && up.OrgId == payload.OrgId, cnt);
            
        if (alreadyInOrg) return "Вы уже состоите в этой организации";

        var correctRoleName = inviteOrg.PossiblePositions
            .Where(p => p != null)
            .First(p => p.Trim().Equals(payload.Role?.Trim(), StringComparison.OrdinalIgnoreCase));

        dbContext.UserPositions.Add(new UserPosition(
            selfId: Guid.NewGuid(),  
            userId: cmd.UserId,      
            orgId: payload.OrgId,   
            posName: correctRoleName
        ));

        if (payload.IsOneTime)
            dbContext.OrganisationInviteCodes.Remove(invite);

        await dbContext.SaveChangesAsync(cnt);
        return "Успешное вступление!";
    }
}