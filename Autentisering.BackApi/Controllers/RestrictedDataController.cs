using Autentisering.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Autentisering.BackApi.Controllers
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
            var identity = this.HttpContext.User.Identities.First();
            var name = identity.Claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
            var jti = identity.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();



            RestrictedData restrictedData = new RestrictedData()
            {
                Name = name,
                Value = counter++,
                Jti= jti
            };

            return await Task.FromResult(restrictedData);
        }
    }
}