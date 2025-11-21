using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly JwtSettings _settings ;
    public JwtService(JwtSettings settings)
    {
        _settings = settings ;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier , user.UserID.ToString()),
            new Claim(ClaimTypes.Name , user.Username),
            new Claim(ClaimTypes.Role , user.Role.ToString())
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer   : _settings.Issuer   ,
            audience : _settings.Audience ,
            claims   : claims ,
            expires  : DateTime.UtcNow.AddHours(_settings.ExpiresInHours),
            signingCredentials : creds 
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}