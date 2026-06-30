using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DOCSeal.Infrastructure.Services.InviteCodeGenerator;

public class InviteCodeGenerator(IConfiguration config):IInviteCodeGenerator
{
    public string GenerateToken(Guid orgId, string role, string? targetEmail, bool isOneTime, int durationDays)
    {
        var secretKey = config["JwtSettings:SecretKey"] 
            ?? throw new Exception("JwtSettings:SecretKey не настроен");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("org_id", orgId.ToString()),
            new("role", role),
            new("is_one_time", isOneTime.ToString().ToLower()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (!string.IsNullOrEmpty(targetEmail))
            claims.Add(new Claim("target_email", targetEmail));

        var expires = durationDays > 0 
            ? DateTime.UtcNow.AddDays(durationDays) 
            : DateTime.UtcNow.AddYears(10);

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    
    
    public InvitePayload ValidateToken(string token)
    {
        var secretKey = config["JwtSettings:SecretKey"] 
            ?? throw new Exception("JwtSettings:SecretKey не настроен");
        
        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var orgIdStr = principal.FindFirst("org_id")?.Value;
        var role = principal.FindFirst("role")?.Value;
        var targetEmail = principal.FindFirst("target_email")?.Value;
        var isOneTimeStr = principal.FindFirst("is_one_time")?.Value;

        if (string.IsNullOrEmpty(orgIdStr) || string.IsNullOrEmpty(role))
            throw new Exception("Неверный формат токена приглашения");

        return new InvitePayload(
            OrgId: Guid.Parse(orgIdStr),
            Role: role,
            TargetEmail: targetEmail,
            IsOneTime: bool.Parse(isOneTimeStr ?? "false")
        );
    }
}