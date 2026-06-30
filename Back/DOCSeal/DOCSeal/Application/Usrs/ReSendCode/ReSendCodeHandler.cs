using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.EmailService;
using DOCSeal.Infrastructure.Services.VerificationCode;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Usrs;

public class ReSendCodeHandler(
    IEmailSender emailSender,
    IVerificationCodeService verificationCodeService,
    AppDbContext dbContext
    ):IRequestHandler<ReSendCodeCommand,string>
{

    public async Task<string> Handle(ReSendCodeCommand cmd, CancellationToken cnt)
    {
        var anon = await dbContext.Users.FirstOrDefaultAsync(x => (x.Email == cmd.email)&&(!x.IsVerified), cnt) ??
                   throw new Exception("Whoops");
        verificationCodeService.RemoveCode(anon.Id);
        await emailSender.SendRegistrationCodeAsync(anon.Email, verificationCodeService.GenerateAndSaveCode(anon.Id));
        
        return $"Код выслан на почту";
    }
}