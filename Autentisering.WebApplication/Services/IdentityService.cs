using Autentisering.WebApplication.ExternalApi;
using Refit;

namespace Autentisering.WebApplication.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> logger;
    private readonly IIdentityApi identityApi;

    public IdentityService(ILogger<IdentityService> logger, IIdentityApi identityApi)
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


    public async Task<string> GetAuthorizationCode(string client_id, string userName, string password)
    {
        var authorizationCode = String.Empty;

        try
        {
            var response = await this.identityApi.GetAuthorizationCode(client_id,userName, password);
            if (response.IsSuccessStatusCode)
            {
                authorizationCode = await response.Content.ReadAsStringAsync();
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

        return authorizationCode;

    }


    public async Task<string> GetIdToken(string authorizationCode)
    {
        var idToken = String.Empty;

        try
        {
            var response = await this.identityApi.GetIdToken(authorizationCode);
            if (response.IsSuccessStatusCode)
            {
                idToken = await response.Content.ReadAsStringAsync();
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

        return idToken;

    }

    public async Task<string> GetAccessToken(string authorizationCode)
    {
        var accessToken = String.Empty;

        try
        {
            var response = await this.identityApi.GetAccessToken(authorizationCode);
            if (response.IsSuccessStatusCode)
            {
                accessToken = await response.Content.ReadAsStringAsync();
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

        return accessToken;

    }

    public async Task<string> GetUserinfo(string AccessToken)
    {
        var userinfo = String.Empty;

        try
        {
            var response = await this.identityApi.GetUserinfo(AccessToken);
            if (response.IsSuccessStatusCode)
            {
                userinfo = await response.Content.ReadAsStringAsync();
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

        return userinfo;

    }

}
