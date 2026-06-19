namespace DOCSeal.Domain.Entities.Users;

public class User : Entity
{
    public string Name { get; set; }
    public List<UserPosition>? Organisations { get; set; }
    public string HashPass { get; set; }
    public string Email { get; set; } 
//  public string PhoneNumber { get; set; }
    public string VerificationCode { get; set; }//Memory Cache
    public DateTime? CodeExpiresAt { get; set; }//Memory Cache
    public bool IsVerified { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    public User(Guid id,string userName, string password, string email,List<UserPosition>? organisations,string verificationCode) : base(id)
    {
        Name = userName;
        HashPass = password;
        Email = email;
//      Phone = phone; , string phone
        Organisations = organisations;
        VerificationCode = verificationCode;
        IsVerified = false;
    }
}