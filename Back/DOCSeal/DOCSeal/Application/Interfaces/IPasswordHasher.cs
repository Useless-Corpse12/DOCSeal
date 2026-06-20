namespace DOCSeal.Application.Interfaces;

public interface IPasswordHasher
{
    string Create(string value);
    bool Validate(string value, string hash);
}