namespace DOCSeal.Infrastructure.Services.InviteCodeGenerator;

public interface IInviteCodeGenerator
{

    string GenerateToken(Guid orgId, string role, string? targetEmail, bool isOneTime, int durationDays);
    InvitePayload ValidateToken(string token);
}

public record InvitePayload(Guid OrgId, string Role, string? TargetEmail, bool IsOneTime );