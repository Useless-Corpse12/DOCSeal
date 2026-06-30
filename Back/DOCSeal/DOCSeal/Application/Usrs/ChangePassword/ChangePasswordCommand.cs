using MediatR;

namespace DOCSeal.Application.Usrs;

public record ChangePasswordCommand(string OldPassword, string NewPassword,Guid Id): IRequest<string>;