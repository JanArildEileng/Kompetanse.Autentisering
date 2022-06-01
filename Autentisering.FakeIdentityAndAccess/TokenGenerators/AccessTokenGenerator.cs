using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autentisering.FakeIdentityAndAccess.TokenGenerators;

public class AccessTokenGenerator
{
    private readonly IConfiguration _config;

    public AccessTokenGenerator(IConfiguration configuration)
    {
        this._config = configuration;
    }

    public string GetAccessToken()
    {
        string JWTToken = BuildJWTToken();
        return JWTToken;
    }

    private string BuildJWTToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["IdJwtToken:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuer = _config["IdJwtToken:Issuer"];
        var audience = _config["IdJwtToken:Audience"];
        var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["IdJwtToken:TokenExpiry"]));

        var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, "Jan"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(issuer,
          audience,
          authClaims,
          expires: jwtValidity,
          signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}