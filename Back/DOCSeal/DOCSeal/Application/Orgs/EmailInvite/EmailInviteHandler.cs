using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using DOCSeal.Infrastructure.Services.EmailService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;



public class EmailInviteHandler(
    AppDbContext dbContext,
    IInviteCodeGenerator codeGenerator,
    IEmailSender emailSender
) : IRequestHandler<EmailInviteCommand, string>
{
    public async Task<string> Handle(EmailInviteCommand cmd, CancellationToken cnt)
    {
        var invite = await dbContext.OrganisationInviteCodes
            .FirstOrDefaultAsync(i => i.InviteCode == cmd.InviteCode, cnt)
            ?? throw new Exception("Код не найден");

        var org = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.Id == invite.OrganisationId, cnt)
            ?? throw new Exception("Организация не найдена");

        var isOwner = await dbContext.UserPositions
            .AnyAsync(up => up.UserId == cmd.RequesterId 
                         && up.OrgId == invite.OrganisationId 
                         && up.PosName == "Big Boss", cnt);

        if (!isOwner)
            throw new Exception("Только владелец может отправлять приглашения");

        var payload = codeGenerator.ValidateToken(cmd.InviteCode);

        var inviteLink = $"http://localhost:5173/invite?code={Uri.EscapeDataString(cmd.InviteCode)}";
        
        var subject = $"Приглашение в организацию {org.Name}";
        var body = $"""
            Здравствуйте!
            
            Вас приглашают в организацию "{org.Name}" на роль "{payload.Role}".
            
            Перейдите по ссылке для вступления:
            {inviteLink}
            
            Или введите код вручную:
            {cmd.InviteCode}
        """;

        await emailSender.SendEmailAsync(cmd.TargetEmail, subject, body);

        return $"Приглашение отправлено на {cmd.TargetEmail}";
    }
}