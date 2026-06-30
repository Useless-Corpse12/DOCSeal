using DOCSeal.Infrastructure.Services.VerificationCode;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Usrs;

public class VerificationHandler(
    AppDbContext dbContext, 
    IVerificationCodeService verificationCodeService
    ):IRequestHandler<VerificationCommand,Guid>

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<Guid> Handle(VerificationCommand cmd,CancellationToken cnt)
    {
        var user =  await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == cmd.Login, cancellationToken: cnt);
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