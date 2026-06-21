using DOCSeal.Application.Interfaces;
using DOCSeal.Domain.Entities.Users;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.DataContext.Exceptions;
using MediatR;

namespace DOCSeal.Application.Features.Users.ChangePassword;

public class ChangePasswordHandler(
    AppDbContext dbContext, 
    IPasswordHasher passwordHasher)
    :IRequestHandler<ChangePasswordCommand, Guid>
{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<Guid> Handle(ChangePasswordCommand cmd,CancellationToken cnt)
    {
        var user = DbContext.Users.FirstOrDefault(x => x.Email == cmd.Login) ?? throw new EntityParamNotFound(typeof(User).ToString(),cmd.Login);
        if (!passwordHasher.Validate(cmd.OldPassword ,user.HashPass))
            throw new Exception("Указан неверный старый пароль");

        user.HashPass = passwordHasher.Create(cmd.NewPassword);
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync();
        return user.Id;
    }
}