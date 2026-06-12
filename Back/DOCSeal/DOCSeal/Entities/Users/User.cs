namespace DOCSeal.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public List<UserPosition>? Organisations { get; set; }
    public string Pass { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public User(string userName, string password, string email, string phone,List<UserPosition>? organisations)
    {
        Name = userName;
        Pass = password;
        Email = email;
        Phone = phone;
        Organisations = organisations;
    }
}