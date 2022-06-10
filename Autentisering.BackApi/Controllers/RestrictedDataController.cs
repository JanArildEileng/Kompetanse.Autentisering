using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Authorization.Shared.Dto.BackEnd;

namespace Authorization.BackendApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RestrictedDataController : ControllerBase
    {

        static int counter = 1;

        private readonly ILogger<RestrictedDataController> _logger;

        public RestrictedDataController(ILogger<RestrictedDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetRestrictedData")]
        public async Task<RestrictedData> Get()
        {
            JwtSecurityToken jwtSecurityToken = GetBearerToken();

            var identity = HttpContext.User.Identities.First();
            var name = identity.Claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
            var jti = identity.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();

            RestrictedData restrictedData = new RestrictedData()
            {
                Name = name,
                Value = counter++,
                Jti = jti,
                ValidTo = jwtSecurityToken?.ValidTo.ToLocalTime()
            };

            return await Task.FromResult(restrictedData);
        }


        private JwtSecurityToken GetBearerToken()
        {
            JwtSecurityToken jwtSecurityToken = null;

            var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = bearerToken.Replace("Bearer ", "");
                jwtSecurityToken = handler.ReadJwtToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return jwtSecurityToken;
        }
    }
}