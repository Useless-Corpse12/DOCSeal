using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DOCSeal.Infrastructure.Services.InviteCodeGenerator;

public class InviteCodeGenerator(IConfiguration config) : IInviteCodeGenerator
{
    public string GenerateToken(Guid orgId, string role, bool isOneTime, int durationDays)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("org_id", orgId.ToString()),
            new("rrole", role),
            new("is_one_time", isOneTime.ToString().ToLower()),
            new("duration_days", durationDays.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

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
        var key = Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!);
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
        }, out _);
        
        
        return new InvitePayload(
            OrgId: Guid.Parse(principal.FindFirst("org_id")?.Value!),
            Role: principal.FindFirst("rrole").Value,
            IsOneTime: bool.Parse(principal.FindFirst("is_one_time")?.Value ?? "false"),
            DurationDays: int.Parse(principal.FindFirst("duration_days")?.Value ?? "0")
        );
    }
}