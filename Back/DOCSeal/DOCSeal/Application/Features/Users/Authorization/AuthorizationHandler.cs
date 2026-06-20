using DOCSeal.Application.Interfaces;
using DOCSeal.Infrastructure.DataContext;

namespace DOCSeal.Application.Features.Users.Authorization;

public class AuthorizationHandler(
    AppDbContext dbContext, 
    IConfiguration configuration, 
    IPasswordHasher passwordHasher, 
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService
    )

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<string> AuthAsync(AuthorizationCommand cmd)
    {
        var user = DbContext.Users.FirstOrDefault(x=>x.Email == cmd.Login);
        if (user == null)
            return "Такого пользователя нет";
        
        return !passwordHasher.Validate(cmd.Password,user.HashPass) ? "пароль или логин не совпадает" : user.Id.ToString();
    }
}