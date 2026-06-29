using DOCSeal.Domain.Entities.Users;
using DOCSeal.Infrastructure.Security.Hasher;
using DOCSeal.Infrastructure.Security.TokenGenerator;
using DOCSeal.Infrastructure.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Users.Authorization;

public record AuthResult(string AccessToken, string RefreshToken);

public class AuthorizationHandler(
    AppDbContext dbContext, 
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator
    ): IRequestHandler<AuthorizationCommand, AuthResult>

{
    private AppDbContext DbContext { get; } = dbContext;
    
    public async Task<AuthResult> Handle(AuthorizationCommand cmd,CancellationToken cnt)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == cmd.Login, cancellationToken: cnt);
        
        if (user == null || !passwordHasher.Validate(cmd.Password, user.HashPass))
            throw new Exception("Неверный логин или пароль");
        
        var accessToken = tokenGenerator.GenerateAccessToken(user);
        var refresh = tokenGenerator.GenerateRefreshToken();
        
        var refreshToken = new RefreshToken(Guid.NewGuid(),user.Id,refresh.Token,refresh.ExpiresAt,cmd.FingerPrint);
        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cnt);
        
        
        return new AuthResult(accessToken, refresh.Token);
    }
}