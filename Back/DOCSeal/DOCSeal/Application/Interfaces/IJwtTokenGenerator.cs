using DOCSeal.Domain.Entities.Users;

namespace DOCSeal.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}