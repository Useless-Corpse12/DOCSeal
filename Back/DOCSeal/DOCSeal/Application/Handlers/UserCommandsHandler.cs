using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.DataContext.Exceptions;
using DOCSeal.Infrastructure.Services.EmailService;
using DOCSeal.Application.Commands.Users;
using DOCSeal.Infrastructure.Security;

namespace DOCSeal.Application.Handlers;


public class UserCommandsHandler(AppDbContext dbContext, IConfiguration configuration, IPasswordHasher passwordHasher, IEmailSender emailSender)
{
    private AppDbContext DbContext { get; } = dbContext;
    public async Task<string> RegisterSelfAsync(RegistrationSelfCommand cmd)
    {
        
        
        
    }

    public async Task<string> AuthAsync(AuthorizationCommand cmd)
    {
        
    }

    public async Task<string> VerifyAsync(VerificationCommand cmd)
    {
    
        
        
    }

    public async Task<string> ChangePassAsync(string? userId, ChangePassCommand cmd)
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