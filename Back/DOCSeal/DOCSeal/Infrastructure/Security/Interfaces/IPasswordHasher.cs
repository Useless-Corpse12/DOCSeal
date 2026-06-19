namespace DOCSeal.Infrastructure.Security;

public interface IPasswordHasher
{
    string Create(string value);
    bool Validate(string value, string hash);
}