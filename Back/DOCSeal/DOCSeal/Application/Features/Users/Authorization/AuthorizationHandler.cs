using DOCSeal.Application.Interfaces;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Features.Users.Authorization;

public class AuthorizationHandler(
    AppDbContext dbContext, 
    IPasswordHasher passwordHasher
    ): IRequestHandler<AuthorizationCommand, Guid>

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<Guid> Handle(AuthorizationCommand cmd,CancellationToken cnt)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == cmd.Login);
        if (user == null)
            throw new Exception("Неверный логин или пароль");

        if (!passwordHasher.Validate(cmd.Password, user.HashPass))
            throw new Exception("Неверный логин или пароль");
        
        return user.Id;
    }
}