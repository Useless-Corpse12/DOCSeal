using DOCSeal.Infrastructure.Security.Hasher;
using DOCSeal.Infrastructure.Services.EmailService;
using DOCSeal.Infrastructure.Services.VerificationCode;
using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Usrs;

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
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == cmd.Email, cancellationToken: cnt);
        
        if (existingUser != null)
        {
            if (existingUser.IsVerified)
                throw new Exception("Пользователь с таким email уже зарегистрирован");
            
            existingUser.Name = cmd.Name;
            existingUser.Phone = cmd.Phone;
            existingUser.HashPass = passwordHasher.Create(cmd.Password);
            
            verificationCodeService.RemoveCode(existingUser.Id);
            var code = verificationCodeService.GenerateAndSaveCode(existingUser.Id);
            
            dbContext.Users.Update(existingUser);
            await dbContext.SaveChangesAsync(cnt);
            
            // await emailSender.SendRegistrationCodeAsync(existingUser.Email, code);
            Console.WriteLine($"Обновлён пользователь. Новый код: {code}");
            
            return existingUser.Id;
        }
        
        var user = new User(
            id: Guid.NewGuid(),
            userName: cmd.Name,
            password: passwordHasher.Create(cmd.Password),
            phone: cmd.Phone,
            email: cmd.Email
        );
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cnt);
        
        var newCode = verificationCodeService.GenerateAndSaveCode(user.Id);
        
        // await emailSender.SendRegistrationCodeAsync(user.Email, newCode);
        Console.WriteLine($"Создан пользователь. Код: {newCode}");
        
        return user.Id;
    }
}