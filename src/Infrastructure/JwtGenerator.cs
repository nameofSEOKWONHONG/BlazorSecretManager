using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BlazorSecretManager.Infrastructure;

public class JwtGenerator
{
    public static string GenerateJwtToken(DateTime? expire, string id, string email, string name, string userKey, string phone, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.PrimarySid, userKey),
            new Claim(ClaimTypes.MobilePhone, phone),
            new Claim(ClaimTypes.Role, role ?? string.Empty),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECURITY_KEY")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable("ISSUER"),
            audience: Environment.GetEnvironmentVariable("AUDIENCE"),
            claims: claims,
            expires: expire,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}