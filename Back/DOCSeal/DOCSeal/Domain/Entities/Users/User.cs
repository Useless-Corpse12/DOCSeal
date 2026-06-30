namespace DOCSeal.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string HashPass { get; set; }
    public string Email { get; set; } 
    public string Phone { get; set; }
    public bool IsVerified { get; set; }

    public User(Guid id,string userName, string password, string email , string phone) : base(id)
    {
        Name = userName;
        HashPass = password;
        Email = email;
        Phone = phone;
        IsVerified = false;
    }
    
    protected User(){}
}