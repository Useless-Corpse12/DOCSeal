namespace DOCSeal.Application.Interfaces;

public interface IVerificationCodeService
{
    string GenerateAndSaveCode(Guid userId);
    bool ValidateCode(Guid userId, string code);
    void RemoveCode(Guid userId);
}