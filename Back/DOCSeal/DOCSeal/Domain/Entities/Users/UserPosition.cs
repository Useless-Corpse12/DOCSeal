namespace DOCSeal.Domain.Entities;

public class UserPosition : Entity
{
    public string PosName { get; set; }
    public Guid UserId { get; set; }
    public Guid OrgId { get; set; }
    
    public UserPosition(Guid selfId,Guid userId,Guid orgId, string posName ) : base(selfId)
    {
        UserId = userId;
        OrgId = orgId;
        PosName = posName;
    }
    
    protected UserPosition(){}
}