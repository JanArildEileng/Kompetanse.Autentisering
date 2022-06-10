using Authorization.Shared.Dto.IdentityAndAccess;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.FakeIdentityAndAccess.Services.TokenGenerators;

public class RefreshTokenGenerator
{
    private readonly IConfiguration _config;

    public RefreshTokenGenerator(IConfiguration configuration)
    {
        _config = configuration;
    }

    public string GetFreshToken(User user)
    {
        string JWTToken = BuildJWTToken(user);
        return JWTToken;
    }

    private string BuildJWTToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["RefreshJwtToken:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuer = _config["RefreshJwtToken:Issuer"];
        var audience = _config["RefreshJwtToken:Audience"];

        var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["RefreshJwtToken:TokenExpiry"]));

        var authClaims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Jti, user.Guid.ToString()),
        };

        var token = new JwtSecurityToken(issuer,
          audience,
          authClaims,
          expires: jwtValidity,
          signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}