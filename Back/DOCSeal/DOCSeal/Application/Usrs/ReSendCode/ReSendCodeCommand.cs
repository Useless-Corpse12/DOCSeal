using MediatR;

namespace DOCSeal.Application.Usrs;

public record ReSendCodeCommand(string email):IRequest<string>;