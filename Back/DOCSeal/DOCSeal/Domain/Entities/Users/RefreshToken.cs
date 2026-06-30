namespace DOCSeal.Domain.Entities;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
    public string BrowserFingerPrint { get; set; }

    public RefreshToken(Guid selfId,Guid userId, string token, DateTime expiring, string fingerprint) : base(selfId)
    {
        UserId = userId;
        Token = token;
        RefreshTokenExpires = expiring;
        BrowserFingerPrint = fingerprint;
    }
    
    protected RefreshToken(){}
}