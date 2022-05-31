using Autentisering.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Autentisering.BackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestrictedDataController : ControllerBase
    {
      

        private readonly ILogger<RestrictedDataController> _logger;

        public RestrictedDataController(ILogger<RestrictedDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetRestrictedData")]
        public async Task<RestrictedData> Get()
        {
            RestrictedData restrictedData = new RestrictedData()
            {
                Name = "Test",
                Value = 1
            };

            return await Task.FromResult(restrictedData);
        }
    }
}