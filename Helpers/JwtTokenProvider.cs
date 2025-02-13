using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Models;

namespace Backend.Helpers;
public class JwtTokenProvider
{
    private readonly JwtConfiguration _config;

    public JwtTokenProvider(JwtConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string id, string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id),
            new Claim(JwtRegisteredClaimNames.Email, email),
            // Add more claims if needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_config.ExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public bool ValidateToken(string token, string id, string email)
    {
        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadJwtToken(token);
        return (
            tokenS.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value == email &&
            tokenS.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value == id);
    }
}