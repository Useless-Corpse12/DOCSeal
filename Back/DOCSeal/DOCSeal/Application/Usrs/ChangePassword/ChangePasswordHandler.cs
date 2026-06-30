using DOCSeal.Infrastructure.Security.Hasher;
using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.DataContext.Exceptions;
using MediatR;

namespace DOCSeal.Application.Usrs;

public class ChangePasswordHandler(
    AppDbContext dbContext, 
    IPasswordHasher passwordHasher)
    :IRequestHandler<ChangePasswordCommand, string>
{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<string> Handle(ChangePasswordCommand cmd,CancellationToken cnt)
    {
        var user = DbContext.Users.FirstOrDefault(x => x.Id == cmd.Id) ?? throw new EntityParamNotFound(typeof(User).ToString(),cmd.Id.ToString());
        if (!passwordHasher.Validate(cmd.OldPassword ,user.HashPass))
            throw new Exception("Указан неверный старый пароль");

        user.HashPass = passwordHasher.Create(cmd.NewPassword);
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync(cnt);
        return "Great";
    }
}