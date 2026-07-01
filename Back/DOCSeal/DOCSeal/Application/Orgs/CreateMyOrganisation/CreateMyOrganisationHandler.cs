using DOCSeal.Infrastructure.DataContext;
using MediatR;
using DOCSeal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class CreateMyOrganisationHandler(AppDbContext dbContext):IRequestHandler<CreateMyOrganisationCommand,Guid>
{
    public async Task<Guid> Handle(CreateMyOrganisationCommand cmd, CancellationToken cnt)
    {
        var ownerUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == cmd.OwnerId, cnt) ??
                        throw new Exception("Такого успера нема");

        var createdOrganisation = new Organisation
        (
            id: Guid.NewGuid(),
            name: cmd.Name,
            possiblePositions:new List<string>{"Big Boss","Employee"}
        );

        var newPos = new UserPosition
        (
            selfId: Guid.NewGuid(),
            orgId: createdOrganisation.Id,
            userId: ownerUser.Id,
            posName: "Big Boss"
        );
    
        dbContext.Organisations.Add(createdOrganisation);
        dbContext.UserPositions.Add(newPos);
        await dbContext.SaveChangesAsync(cnt);
        return createdOrganisation.Id;
    }
}