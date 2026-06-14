namespace DOCSeal.Entities;

public class UserPosition
{
    public string PosName { get; set; }
    public Guid OrgId { get; set; }
    
    public UserPosition(Guid orgId, string posName )
    {
        OrgId = orgId;
        PosName = posName;
    }
}