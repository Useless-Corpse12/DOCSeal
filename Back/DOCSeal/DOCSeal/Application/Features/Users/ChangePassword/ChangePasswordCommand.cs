using MediatR;

namespace DOCSeal.Application.Features.Users.ChangePassword;

public record ChangePasswordCommand(Guid Id,string OldPassword, string NewPassword): IRequest<string>;