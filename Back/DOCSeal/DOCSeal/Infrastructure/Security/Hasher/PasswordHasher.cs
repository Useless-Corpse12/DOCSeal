namespace DOCSeal.Infrastructure.Security.Hasher;
using DOCSeal.Application.Interfaces;

public class PasswordHasher(string salt) : IPasswordHasher
{
    public string Create(string value)
    {
        return Hasher.Create(value, salt);
    }

    public bool Validate(string value, string hash)
    {
        return Hasher.Validate(value, salt,hash);
    }
}