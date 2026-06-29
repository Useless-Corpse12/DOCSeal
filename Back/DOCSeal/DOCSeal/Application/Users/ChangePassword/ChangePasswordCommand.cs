using MediatR;

namespace DOCSeal.Application.Users.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword,Guid Id): IRequest<string>;