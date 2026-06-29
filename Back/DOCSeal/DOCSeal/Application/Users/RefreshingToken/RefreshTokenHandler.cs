using DOCSeal.Domain.Entities.Users;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.Security.TokenGenerator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DOCSeal.Application.Users.RefreshingToken;

public record AuthResult(string AccessToken, string RefreshToken);


public class RefreshTokenHandler(
    AppDbContext dbContext,
    ITokenGenerator tokenGenerator
) : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RefreshTokenCommand cmd, CancellationToken cnt)
    {
        var session = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == cmd.RefreshToken, cnt);
        
        if (session == null)
            throw new Exception("Неверный Refresh Token");
        
        if (session.RefreshTokenExpires < DateTime.UtcNow)
        {
            await dbContext.RefreshTokens.Where(t => t.Id == session.Id).ExecuteDeleteAsync(cnt);
            throw new Exception("Refresh Token истёк");
        }
        
        if (session.BrowserFingerPrint != cmd.FingerPrint)
        {
            await dbContext.RefreshTokens
                .Where(t => t.UserId == session.UserId)
                .ExecuteDeleteAsync(cnt);
            
            throw new Exception("Требуется новый вход.");
        }
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == session.UserId, cnt);
        if (user == null)
            throw new Exception("Пользователь не найден");
        
        await dbContext.RefreshTokens.Where(t => t.Id == session.Id).ExecuteDeleteAsync(cnt);
        
        var newAccessToken = tokenGenerator.GenerateAccessToken(user);
        var newRefreshResult = tokenGenerator.GenerateRefreshToken();
        
        var newSession = new RefreshToken(
            Guid.NewGuid(),
            user.Id,
            newRefreshResult.Token,
            newRefreshResult.ExpiresAt,
            cmd.FingerPrint
        );
        
        dbContext.RefreshTokens.Add(newSession);
        await dbContext.SaveChangesAsync(cnt);
        
        return new AuthResult(newAccessToken, newRefreshResult.Token);
    }
}