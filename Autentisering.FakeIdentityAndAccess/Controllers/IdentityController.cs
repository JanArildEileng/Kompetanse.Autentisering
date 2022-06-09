using Autentisering.FakeIdentityAndAccess.TokenGenerators;
using Autentisering.FakeIdentityAndAccess.TokenValidators;
using Autentisering.Shared.Dto.IdentityAndAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;

namespace Autentisering.FakeIdentityAndAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;

        public AuthorizationCodeCache authorizationCodeCache { get; }

        public IdentityController(ILogger<IdentityController> logger, AuthorizationCodeCache authorizationCodeCache)
        {
            _logger = logger;
            this.authorizationCodeCache = authorizationCodeCache;
        }

        [HttpGet(Name = "GetIdentity")]
        public User GetIdentity(string jti, [FromServices] UserRepoitory userRepoitory)
        {
            User user = userRepoitory.GetUser(Guid.Parse(jti));
            return user;
        }

        [HttpGet("All", Name = "GetAllIdentity")]
        public List<User> GetAllIdentity([FromServices] UserRepoitory userRepoitory)
        {
            var users = userRepoitory.GetAllUser();
            return users;
        }







        [HttpGet("/Userinfo", Name = "GetUserinfo")]
        public string GetUserinfo(string access_token)
        {
            string userinfo = String.Empty;
            return userinfo;
        }






    }
}