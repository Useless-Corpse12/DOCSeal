using DOCSeal.Application.Interfaces;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.DataContext.Exceptions;

namespace DOCSeal.Application.Features.Users.ChangePassword;

public class ChangePasswordHandler(
    AppDbContext dbContext, 
    IConfiguration configuration,
    IPasswordHasher passwordHasher)
{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<string> ChangePasswordAsync(string? userId, ChangePasswordCommand cmd)
    {
        var id = Guid.Parse(userId ?? throw new  EntityParamNotFound("User","id"));
        var user = DbContext.Users.FirstOrDefault(x => x.Id == id) ?? throw new EntityInstanceIdNotFound("User",id);
        if (!passwordHasher.Validate(user.HashPass ,cmd.OldPassword))
            return "Указан неверный старый пароль";

        user.HashPass = passwordHasher.Create(cmd.NewPassword);
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync();
        return "Пароль изменен";
    }
}