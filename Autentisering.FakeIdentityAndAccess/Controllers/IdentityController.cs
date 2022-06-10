using Authorization.FakeIdentityAndAccess.AppServices.Contracts;
using Authorization.FakeIdentityAndAccess.Services.AuthorizationCode;
using Authorization.Shared.Dto.IdentityAndAccess;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.FakeIdentityAndAccess.Controllers
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
        public User GetIdentity(string jti, [FromServices] IUserRepoitory userRepoitory)
        {
            User user = userRepoitory.GetUser(Guid.Parse(jti));
            return user;
        }

        [HttpGet("All", Name = "GetAllIdentity")]
        public List<User> GetAllIdentity([FromServices] IUserRepoitory userRepoitory)
        {
            var users = userRepoitory.GetAllUser();
            return users;
        }







        [HttpGet("/Userinfo", Name = "GetUserinfo")]
        public string GetUserinfo(string access_token)
        {
            string userinfo = string.Empty;
            return userinfo;
        }






    }
}