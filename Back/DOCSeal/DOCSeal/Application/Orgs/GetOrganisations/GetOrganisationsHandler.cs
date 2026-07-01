using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class GetOrganisations(AppDbContext dbContext) : IRequestHandler<GetOrganisationsCommand, GettingOrganisationsResponse>
{
    public async Task<GettingOrganisationsResponse> Handle(GetOrganisationsCommand cmd, CancellationToken cnt)
    {
        var positions = await dbContext.UserPositions
            .Where(x => x.UserId == cmd.RequesterId)
            .ToListAsync(cnt);

        var orgIds = positions.Select(p => p.OrgId).ToList();

        var orgs = await dbContext.Organisations
            .Where(o => orgIds.Contains(o.Id))
            .ToListAsync(cnt);

        var result = orgs.Select(org => 
        {
            var isOwner = positions.Any(p => p.OrgId == org.Id && p.PosName == "Big Boss");
            return new OrgBrief(org.Id, org.Name, isOwner);
        }).ToList();

        return new GettingOrganisationsResponse(result);
    }
}