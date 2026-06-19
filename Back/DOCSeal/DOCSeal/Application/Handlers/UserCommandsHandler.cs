  //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
using DOCSeal.Infrastructure.DataContext.Exceptions;
using DOCSeal.Infrastructure.DataContext;
//=============================================================|"|
using DOCSeal.Infrastructure.Services.EmailService;
using DOCSeal.Infrastructure.Services.VerificationCode;
//=============================================================|"|
//=============================================================|"|
using DOCSeal.Application.Commands.Users;
//=============================================================|"|
//=============================================================|"|
using DOCSeal.Domain.Entities.Users;
//=============================================================|"|
//=============================================================|"|
using DOCSeal.Infrastructure.Security;                                                        //  
//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//    
namespace DOCSeal.Application.Handlers;


public class UserCommandsHandler(
    AppDbContext dbContext, 
    IConfiguration configuration, 
    IPasswordHasher passwordHasher, 
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService)

{
    private AppDbContext DbContext { get; } = dbContext;
    
    
    public async Task<string> RegisterSelfAsync(RegistrationSelfCommand cmd)
    {
        var existing = dbContext.Users.Any(x => x.Email == cmd.Email);
        if (!existing)
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
        dbContext.Users.Add(user);

        await emailSender.SendRegistrationCodeAsync(user.Email,verificationCodeService.GenerateAndSaveCode(user.Id));
        return "Регистрация успешна! Код для активации выслан на почту";
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