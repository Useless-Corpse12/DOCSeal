using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public record GetOrganisationsCommand(Guid RequesterId) : IRequest<GettingOrganisationsResponse>;

public record GettingOrganisationsResponse(List<Guid> MyOrgs, List<Guid> MyOwnOrgs);

public class GetOrganisations(AppDbContext dbContext) : IRequestHandler<GetOrganisationsCommand, GettingOrganisationsResponse>
{
    public async Task<GettingOrganisationsResponse> Handle(GetOrganisationsCommand cmd, CancellationToken cnt)
    {
        var requesterPositions = await dbContext.UserPositions
            .Where(x => x.UserId == cmd.RequesterId)
            .ToListAsync<UserPosition>(cnt);
        
        var myOwnOrgs = requesterPositions
            .Where(x => x.PosName == "Big Boss")
            .Select(x => x.OrgId)
            .ToList();

        var myOrgs = requesterPositions
            .Where(x => x.PosName != "Big Boss")
            .Select(x => x.OrgId)
            .ToList();
        
        return new GettingOrganisationsResponse(myOrgs, myOwnOrgs);
    }

}