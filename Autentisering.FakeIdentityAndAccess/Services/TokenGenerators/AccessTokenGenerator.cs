using Authorization.Shared.Dto.IdentityAndAccess;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.FakeIdentityAndAccess.Services.TokenGenerators;

public class AccessTokenGenerator
{
    private readonly IConfiguration _config;

    public AccessTokenGenerator(IConfiguration configuration)
    {
        _config = configuration;
    }

    public string GetAccessToken(User user)
    {
        string JWTToken = BuildJWTToken(user);
        return JWTToken;
    }

    private string BuildJWTToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AccessJwtToken:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuer = _config["AccessJwtToken:Issuer"];
        var audience = _config["AccessJwtToken:Audience"];
        var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["AccessJwtToken:TokenExpiry"]));

        var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Name),
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