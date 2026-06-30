using DOCSeal.Domain.Entities;

namespace DOCSeal.Infrastructure.Security.TokenGenerator;

public record TokenResult(string Token, DateTime ExpiresAt);

public interface ITokenGenerator
{
    string GenerateAccessToken(User user);
    TokenResult GenerateRefreshToken();
}