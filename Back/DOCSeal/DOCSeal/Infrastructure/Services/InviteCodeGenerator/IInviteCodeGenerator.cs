namespace DOCSeal.Infrastructure.Services.InviteCodeGenerator;

public interface IInviteCodeGenerator
{
    string GenerateToken(Guid orgId, string role, bool isOneTime, int durationDays);
    InvitePayload ValidateToken(string token);
}

public record InvitePayload(
    Guid OrgId, 
    string Role, 
    bool IsOneTime,
    int DurationDays
);