using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Usrs;

public record GetUserInfoCommand(Guid Id): IRequest<UserInfo>;

public record UserInfo(string Name, string Email, string Phone);

public class GetUserInfo(AppDbContext dbContext):IRequestHandler<GetUserInfoCommand,UserInfo>
{
    private AppDbContext DbContext { get; } = dbContext;

    public async Task<UserInfo> Handle(GetUserInfoCommand cmd, CancellationToken cnt)
    {
        var searchedUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == cmd.Id , cancellationToken: cnt)?? 
                           throw new Exception("UserProfileSeacrhIsBroken");
        
        return new UserInfo(searchedUser.Name, searchedUser.Email, searchedUser.Phone);
    }
}