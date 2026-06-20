namespace DOCSeal.Application.Features.Users.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword);