namespace DOCSeal.Domain.Entities.Documents;

public class Document : Entity
{
    public Guid OrganisationId { get; set; }
    public Guid SenderId { get; set; }
    public List<Guid> SignerIds { get; set; }
    public List<Guid> RecipientIds { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string ContentUrl { get; set; }
    public string? Message { get; set; }
    public DocumentStatus Status { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateStatusChanged { get; set; }

    public Document(Guid organisationId, Guid senderId, List<Guid> signerIds, List<Guid> recipientIds, string title, string? description, string contentUrl, string? message, DocumentStatus status, DateTime dateCreated, DateTime dateStatusChanged)
    {
        OrganisationId = organisationId;
        SenderId = senderId;
        SignerIds = signerIds;
        RecipientIds = recipientIds;
        Title = title;
        Description = description;
        ContentUrl = contentUrl;
        Message = message;
        Status = status;
        DateCreated = dateCreated;
        DateStatusChanged = dateStatusChanged;
    }
}