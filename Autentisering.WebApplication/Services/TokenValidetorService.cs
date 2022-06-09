using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autentisering.WebBFFApplication.Services
{
    public class TokenValidetorService
    {
        private readonly IConfiguration _config;

        public TokenValidetorService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public JwtSecurityToken ReadValidateIdToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["IdJwtToken:SecretKey"])),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _config["IdJwtToken:Issuer"],
                ValidAudience = _config["IdJwtToken:Audience"]
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
