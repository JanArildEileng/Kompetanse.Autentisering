using Autentisering.WebApplication.ExternalApi;
using Refit;

namespace Autentisering.WebApplication.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> logger;
    private readonly IdentityApi identityApi;

    public IdentityService(ILogger<IdentityService> logger, IdentityApi identityApi)
    {
        this.logger = logger;
        this.identityApi = identityApi;
    }
    public async Task<string> Login(string userName = "TestUSer", string password = "Password")
    {
        var identityName = String.Empty;

        try
        {
            var response = await this.identityApi.GetIdentityHttpResponseMessage(userName, password);
            if (response.IsSuccessStatusCode)
            {
                identityName = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Identity failed " + response.ReasonPhrase);
            }
               
        }
        catch (ApiException apiException)
        {
            logger.LogError(" ApiException {Message} ", apiException.Message);
           
            throw;
        }
        catch (Exception exp)
        {
            logger.LogError(" Exception {Message} ", exp.Message);
            throw;
        }

        return identityName;

    }
}
