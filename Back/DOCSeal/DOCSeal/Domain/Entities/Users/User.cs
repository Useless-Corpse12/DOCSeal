namespace DOCSeal.Domain.Entities.Users;

public class User : Entity
{
    public string Name { get; set; }
    public List<UserPosition>? Organisations { get; set; }
    public string HashPass { get; set; }
    public string Email { get; set; } 
    public string Phone { get; set; }
    public bool IsVerified { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    public User(Guid id,string userName, string password, string email , string phone) : base(id)
    {
        Name = userName;
        HashPass = password;
        Email = email;
        Phone = phone;
        Organisations = new List<UserPosition>();
        IsVerified = false;
    }
    
    protected User() { }
}