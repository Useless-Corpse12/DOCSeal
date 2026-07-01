using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class GetOrganisationInfoHandler(AppDbContext dbContext) : IRequestHandler<GetOrganisationInfoCommand, OrganisationInfoResponse>
{
    public async Task<OrganisationInfoResponse> Handle(GetOrganisationInfoCommand cmd, CancellationToken cnt)
    {
        var org = await dbContext.Organisations
                      .FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt)
                  ?? throw new Exception("Организация не найдена");

        var userPosition = await dbContext.UserPositions
            .FirstOrDefaultAsync(p => p.UserId == cmd.RequesterId && p.OrgId == cmd.OrgId, cnt);

        if (userPosition == null)
            throw new Exception("У вас нет доступа к этой организации");

        var isOwner = userPosition.PosName == "Big Boss";

        var positions = await dbContext.UserPositions
            .Where(p => p.OrgId == cmd.OrgId)
            .ToListAsync(cnt);

        var userIds = positions.Select(p => p.UserId).ToList();
        var users = await dbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cnt);

        var employees = positions.Select(p => {
            var user = users.First(u => u.Id == p.UserId);
            return new EmployeeInfo(
                p.UserId,
                user.Name,
                user.Email,
                p.PosName
            );
        }).ToList();

        return new OrganisationInfoResponse(
            org.Id,
            org.Name,
            isOwner,
            org.PossiblePositions,
            employees
        );
    }
}