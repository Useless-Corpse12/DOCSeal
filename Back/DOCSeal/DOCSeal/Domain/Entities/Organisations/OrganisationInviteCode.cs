namespace DOCSeal.Domain.Entities;

public class OrganisationInviteCode:Entity
{
    public Guid OrganisationId { get; set; }
    public string InviteCode { get; set; }
    
    public OrganisationInviteCode(Guid selfId,Guid orgId, string inviteCode):base(selfId)
    {
        OrganisationId = orgId;
        InviteCode = inviteCode;
    }
    
    protected OrganisationInviteCode()
    {}
}