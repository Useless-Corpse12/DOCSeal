using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class GetInviteInfoHandler(
    AppDbContext dbContext,
    IInviteCodeGenerator codeGenerator
) : IRequestHandler<GetInviteInfoCommand, InviteInfoResponse>
{
    public async Task<InviteInfoResponse> Handle(GetInviteInfoCommand cmd, CancellationToken cnt)
    {
        InvitePayload payload;
        try { payload = codeGenerator.ValidateToken(cmd.Code); }
        catch { return new InviteInfoResponse(false, "", ""); }

        var exists = await dbContext.OrganisationInviteCodes.AnyAsync(i => i.InviteCode == cmd.Code, cnt);
        if (!exists) return new InviteInfoResponse(false, "", "");

        var org = await dbContext.Organisations.FirstOrDefaultAsync(o => o.Id == payload.OrgId, cnt);
        return org == null ? new InviteInfoResponse(false, "", "") : new InviteInfoResponse(true, org.Name, payload.Role);
    }
}