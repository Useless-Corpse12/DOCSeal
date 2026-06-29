using DOCSeal.Infrastructure.Services.VerificationCode;
using DOCSeal.Infrastructure.DataContext;
using MediatR;

namespace DOCSeal.Application.Users.Verification;

public class VerificationHandler(
    AppDbContext dbContext, 
    IVerificationCodeService verificationCodeService
    ):IRequestHandler<VerificationCommand,Guid>

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<Guid> Handle(VerificationCommand cmd,CancellationToken cnt)
    {
        var user = DbContext.Users.FirstOrDefault(x=>x.Email == cmd.Login);
        if (user == null)
            throw new Exception("Такого пользователя нет");

        if (!verificationCodeService.ValidateCode(user.Id, cmd.VerificationCode))
            throw new Exception("Неверный код");
        
        verificationCodeService.RemoveCode(user.Id);
        user.IsVerified = true;
        
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync(cnt);
        
        return user.Id;
    }
}