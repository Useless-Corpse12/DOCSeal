namespace DOCSeal.Domain.Entities.Users;

public class UserPosition : Entity
{
    public string PosName { get; set; }
    public Guid OrgId { get; set; }
    
    public UserPosition(Guid orgId, string posName )
    {
        OrgId = orgId;
        PosName = posName;
    }
}