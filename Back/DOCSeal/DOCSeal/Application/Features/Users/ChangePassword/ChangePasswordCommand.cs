using MediatR;

namespace DOCSeal.Application.Features.Users.ChangePassword;

public record ChangePasswordCommand(string Login,string OldPassword, string NewPassword): IRequest<Guid>;