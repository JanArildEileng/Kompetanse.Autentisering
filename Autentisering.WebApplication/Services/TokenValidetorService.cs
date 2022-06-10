using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autentisering.WebBFFApplication.Services;

public class TokenValidetorService
{
    private readonly TokenValidationParameters tokenValidationParameters;

    public TokenValidetorService(TokenValidationParameters tokenValidationParameters)
    {
        this.tokenValidationParameters = tokenValidationParameters;
    }

    public JwtSecurityToken ReadValidateIdToken(string token)
    {
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
