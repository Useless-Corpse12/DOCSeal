using MediatR;

namespace DOCSeal.Application.Orgs;

public record MultiEmailInviteCommand(
    Guid RequesterId, 
    string InviteCode, 
    List<string> Emails
) : IRequest<int>;