using DOCSeal.Application.Interfaces;
using DOCSeal.Infrastructure.DataContext;

namespace DOCSeal.Application.Features.Users.Verification;

public class VerificationHandler(
    AppDbContext dbContext, 
    IConfiguration configuration, 
    IPasswordHasher passwordHasher, 
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService
    )

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<string> VerifyAsync(VerificationCommand cmd)
    {
        var user = DbContext.Users.FirstOrDefault(x=>x.Email == cmd.Login);
        if (user == null)
            return "Такого пользователя нет";

        if (!verificationCodeService.ValidateCode(user.Id, cmd.VerificationCode))
            return "Неверный код";
        
        verificationCodeService.RemoveCode(user.Id);
        user.IsVerified = true;
        
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync();
        
        return "Аккаунт верифицирован!";
    }
}