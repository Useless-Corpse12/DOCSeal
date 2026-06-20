using DOCSeal.Application.Interfaces;
using DOCSeal.Domain.Entities.Users;
using DOCSeal.Infrastructure.DataContext;

namespace DOCSeal.Application.Features.Users.RegistrationSelf;

public class RegistrationSelfHandler(
    AppDbContext dbContext, 
    IConfiguration configuration, 
    IPasswordHasher passwordHasher, 
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService
)

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<string> RegistrationSelfAsync(RegistrationSelfCommand cmd)
    {
        var existing = DbContext.Users.Any(x => x.Email == cmd.Email);
        if (existing)
            throw new Exception("Пользователь с таким email уже зарегистрирован");
        
        //var org = cmd.org
        var user = new User(
            id: Guid.NewGuid(),
            userName: cmd.Name,
            organisations: null,
            password: passwordHasher.Create(cmd.Password),
            email: cmd.Email
            //,phone: request.Phone
        );
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        await emailSender.SendRegistrationCodeAsync(user.Email,verificationCodeService.GenerateAndSaveCode(user.Id));
        return "Регистрация успешна! Код для активации выслан на почту";
    }
}