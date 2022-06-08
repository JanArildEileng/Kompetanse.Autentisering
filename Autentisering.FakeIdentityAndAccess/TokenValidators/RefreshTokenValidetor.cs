using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autentisering.FakeIdentityAndAccess.TokenValidators
{
    public class RefreshTokenValidetor
    {
        private readonly IConfiguration _config;

        public RefreshTokenValidetor(IConfiguration configuration)
        {
            _config = configuration;
        }

        public JwtSecurityToken ReadValidateIdToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["RefreshJwtToken:SecretKey"])),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _config["RefreshJwtToken:Issuer"],
                ValidAudience = _config["RefreshJwtToken:Audience"]
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = handler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = handler.ReadJwtToken(token);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
