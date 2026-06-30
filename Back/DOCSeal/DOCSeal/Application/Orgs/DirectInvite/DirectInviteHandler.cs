using DOCSeal.Domain.Entities;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Services.InviteCodeGenerator;
using DOCSeal.Infrastructure.Services.EmailService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Orgs;

public class DirectInviteHandler(
    AppDbContext dbContext, 
    IInviteCodeGenerator codeGenerator,
    IEmailSender emailSender
) : IRequestHandler<DirectInviteCommand, string>
{
    public async Task<string> Handle(DirectInviteCommand cmd, CancellationToken cnt)
    {
        var isOwner = await dbContext.UserPositions
            .AnyAsync(up => up.UserId == cmd.RequesterId 
                         && up.OrgId == cmd.OrgId 
                         && up.PosName == "Big Boss", cnt);

        if (!isOwner)
            throw new Exception("Только владелец может приглашать");

        var org = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.Id == cmd.OrgId, cnt)
            ?? throw new Exception("Организация не найдена");

        if (!org.PossiblePositions.Contains(cmd.Role))
            throw new Exception($"Роль '{cmd.Role}' не существует");

        var inviteCode = codeGenerator.GenerateToken(
            cmd.OrgId, cmd.Role, cmd.TargetEmail, true, cmd.DurationDays
        );

        dbContext.OrganisationInviteCodes.Add(new OrganisationInviteCode(
            Guid.NewGuid(), cmd.OrgId, inviteCode
        ));
        await dbContext.SaveChangesAsync(cnt);

        var inviteLink = $"http://localhost:5173/invite?code={Uri.EscapeDataString(inviteCode)}";
        
        var subject = $"Приглашение в организацию {org.Name}";
        var body = $"""
            Здравствуйте!
            
            Вас приглашают в организацию "{org.Name}" на роль "{cmd.Role}".
            
            Перейдите по ссылке для вступления:
            {inviteLink}
            
            Или введите код вручную на странице приглашения:
            {inviteCode}
            
            Ссылка действительна {(cmd.DurationDays > 0 ? $"{cmd.DurationDays} дней" : "бессрочно")}.
        """;

        await emailSender.SendEmailAsync(cmd.TargetEmail, subject, body);

        return $"Приглашение отправлено на {cmd.TargetEmail}";
    }
}