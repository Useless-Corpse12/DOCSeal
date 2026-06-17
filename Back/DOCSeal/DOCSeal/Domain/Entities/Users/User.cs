namespace DOCSeal.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public List<UserPosition>? Organisations { get; set; }
    public string Pass { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string VerificationCode { get; set; }//Memory Cache
    public DateTime? CodeExpiresAt { get; set; }//Memory Cache
    public bool IsVerified { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    public User(string userName, string password, string email, string phone,List<UserPosition>? organisations,string verificationCode)
    {
        Name = userName;
        Pass = password;
        Email = email;
        Phone = phone;
        Organisations = organisations;
        VerificationCode = verificationCode;
        IsVerified = false;
    }
}