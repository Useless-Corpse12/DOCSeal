using MediatR;

namespace DOCSeal.Application.Usrs.DeleteMyAccount;

public record DeleteMyAccountCommand(Guid Id,string Password):IRequest;