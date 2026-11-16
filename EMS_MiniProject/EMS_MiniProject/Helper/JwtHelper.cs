using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMS_MiniProject.Models;
using Microsoft.IdentityModel.Tokens;

public static class JwtHelper
{
    public static string GenerateJwtToken(User user, string key, string issuer, string expireMinutes)
    {
        if (string.IsNullOrEmpty(key))
            throw new Exception("JWT Key is missing in configuration");

        if (string.IsNullOrEmpty(issuer))
            throw new Exception("JWT Issuer is missing in configuration");

        if (!int.TryParse(expireMinutes, out int minutes))
            minutes = 60; // default fallback

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
        };

        

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(minutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}