namespace DOCSeal.Application.Interfaces;

public interface IPasswordHasher
{
    string PassHash(string value);
    bool PassValidate(string value, string hash);
}