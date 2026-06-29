using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DOCSeal.Domain.Entities.Users;
using Microsoft.IdentityModel.Tokens;

namespace DOCSeal.Infrastructure.Security.TokenGenerator;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateAccessToken(User user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];
        var expirationMinutes = int.Parse(_configuration["JwtSettings:AccessTokenLifeTimeMins"] ?? "60");//<<<<<токен лайфтайм
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),//<<<<<токен лайфтайм
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenResult GenerateRefreshToken()
    {
        var refreshTokenDays = int.Parse(_configuration["JwtSettings:RefreshTokenLifeTimeDays"] ?? "30");//<<<<<токен лайфтайм
        var expiresAt = DateTime.UtcNow.AddDays(refreshTokenDays);
        
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        
        return new TokenResult(token, expiresAt);
    }
}