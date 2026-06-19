namespace DOCSeal.Application.Commands.Users;

public record ChangePassCommand(string OldPassword, string NewPassword);