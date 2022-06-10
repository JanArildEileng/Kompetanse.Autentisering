using Autentisering.FakeIdentityAndAccess.TokenGenerators;
using Autentisering.Shared.Dto.IdentityAndAccess;
using Common.TokenUtils;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Autentisering.FakeIdentityAndAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtTokenController : ControllerBase
    {
        private readonly ILogger<JwtTokenController> _logger;

        public AuthorizationCodeCache authorizationCodeCache { get; }

        public JwtTokenController(ILogger<JwtTokenController> logger, AuthorizationCodeCache authorizationCodeCache)
        {
            _logger = logger;
            this.authorizationCodeCache = authorizationCodeCache;
        }


        [HttpGet("", Name = "GetToken")]
        public GetTokenResponse GetToken(string authorizationCode, [FromServices] AccessTokenGenerator accessTokenGenerator, [FromServices] IdTokenGenerator idTokenGenerator, [FromServices] RefreshTokenGenerator refreshTokenGenerator)
        {
            GetTokenResponse getTokenResponse = new(); 

            string accessToken = String.Empty;

            if (authorizationCodeCache.TryGet(authorizationCode, out AuthorizationCodeContent authorizationCodeContent))
            {
                //ok...sjekk på client_id?
                //Generer AccessToken..
                User user = authorizationCodeContent.User;

                 getTokenResponse.AccessToken = accessTokenGenerator.GetAccessToken(user);

                //Generer IdToken..
                 getTokenResponse.IdToken = idTokenGenerator.GetIdToken(user);
                //Generer FreshToken
                getTokenResponse.RefreshToken = refreshTokenGenerator.GetFreshToken(user);


                getTokenResponse.Expire = DateTime.Now.AddMinutes(5);
            }
            else
            {
            }

            return getTokenResponse;
        }



        [HttpGet("Refresh", Name = "GetRefreshedTokens")]
        public  ActionResult<GetTokenResponse> GetRefreshedTokens(string refreshToken ,[FromServices] AccessTokenGenerator accessTokenGenerator, [FromServices] RefreshTokenGenerator refreshTokenGenerator, [FromServices] TokenValidetorService tokenValidetorService, [FromServices] UserRepoitory userRepoitory)
        {
            GetTokenResponse getTokenResponse = new();

            string accessToken = String.Empty;

            JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(refreshToken);

            if (jwtSecurityToken == null)
            {
                  return BadRequest($"invalid refreshToken");
            }

            var claims = jwtSecurityToken.Claims.ToList();
            var jti = claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();


            User user = userRepoitory.GetUser(Guid.Parse(jti));

            if (user!=null)
            {
                //ok...sjekk på client_id?
                //Generer AccessToken..
                getTokenResponse.AccessToken = accessTokenGenerator.GetAccessToken(user);
                getTokenResponse.RefreshToken = refreshTokenGenerator.GetFreshToken(user);

                getTokenResponse.Expire = DateTime.Now.AddMinutes(5);
            }
            else
            {
                return BadRequest($"user not found");
  
            }

            return Ok(getTokenResponse);
        }




    }
}