using DOCSeal.Application.Interfaces;

namespace DOCSeal.Infrastructure.Security;

public class PasswordHasher:IPasswordHasher
{
    private readonly string _passwordSalt;

    public PasswordHasher(string salt)
    {
        _passwordSalt = salt;
    }
    
    public string PassHash(string value)
    {
        return Hasher.Create(value, _passwordSalt);
    }

    public bool PassValidate(string value, string hash)
    {
        return Hasher.Validate(value, _passwordSalt,hash);
    }
}