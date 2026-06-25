using DOCSeal.Application.Interfaces;
using DOCSeal.Domain.Entities.Users;
using DOCSeal.Infrastructure.DataContext;
using MediatR;

namespace DOCSeal.Application.Features.Users.RegistrationSelf;

public class RegistrationSelfHandler(
    AppDbContext dbContext, 
    IPasswordHasher passwordHasher, 
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService
    ):IRequestHandler<RegistrationSelfCommand,Guid>

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<Guid> Handle(RegistrationSelfCommand cmd,CancellationToken cnt)
    {
        var existing = DbContext.Users.Any(x => x.Email == cmd.Email);
        if (existing)
            throw new Exception("Пользователь с таким email уже зарегистрирован");
        
        
        var user = new User(
            id: Guid.NewGuid(),
            userName: cmd.Name,
            password: passwordHasher.Create(cmd.Password),
            phone: cmd.Phone,
            email: cmd.Email
        );
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        await emailSender.SendRegistrationCodeAsync(user.Email,verificationCodeService.GenerateAndSaveCode(user.Id));
        return user.Id;
    }
}