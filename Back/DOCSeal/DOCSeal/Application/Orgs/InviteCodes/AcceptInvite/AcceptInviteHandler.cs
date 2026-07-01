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
        
        var invite = await dbContext.OrganisationInviteCodes
                         .FirstOrDefaultAsync(i => i.InviteCode == cmd.Code, cnt)
                     ?? throw new Exception("Инвайт уже использован");

        var inviteOrg = await dbContext.Organisations.FirstAsync
            (or => or.Id == payload.OrgId, cnt) ?? throw new Exception("Оргу снесли пока он заходил, лол");

        if (!inviteOrg.PossiblePositions.Contains(payload.Role)) throw new Exception("steel jobless");

        var alreadyInOrg = await dbContext.UserPositions
            .AnyAsync(up => up.UserId == cmd.UserId && up.OrgId == payload.OrgId, cnt);
            
        if (alreadyInOrg) return "Вы уже состоите в этой организации";

        dbContext.UserPositions.Add(new UserPosition(
            selfId:  Guid.NewGuid(),  
            userId:  cmd.UserId,      
            orgId:   payload.OrgId,   
            posName: payload.Role
        ));

        if (payload.IsOneTime)
            dbContext.OrganisationInviteCodes.Remove(invite);

        await dbContext.SaveChangesAsync(cnt);
        return "Успешное вступление!";
    }
}