using Autentisering.RefitApi.Api;
using Autentisering.Shared.IdentityAndAccess;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http.Json;
using System.Text.Json;

namespace Autentisering.RefitApi.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> logger;
    private readonly IIdentityApi identityApi;

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };


    public IdentityService(ILogger<IdentityService> logger, IIdentityApi identityApi)
    {
        this.logger = logger;
        this.identityApi = identityApi;
    }
    public async Task<string> Login(string userName = "TestUSer", string password = "Password")
    {
        var identityName = string.Empty;

        try
        {
            var response = await identityApi.GetIdentityHttpResponseMessage(userName, password);
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
        var authorizationCode = string.Empty;

        try
        {
            var response = await identityApi.GetAuthorizationCode(client_id, userName, password);
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


   

  

    public async Task<string> GetUserinfo(string AccessToken)
    {
        var userinfo = string.Empty;

        try
        {
            var response = await identityApi.GetUserinfo(AccessToken);
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

    public async Task<GetTokenResponse> GetToken(string authorizationCode)
    {
        GetTokenResponse getTokenResponse =null;

        try
        {
            var response = await identityApi.GetToken(authorizationCode);
            if (response.IsSuccessStatusCode)
            {
                getTokenResponse = JsonSerializer.Deserialize<GetTokenResponse>(await response.Content.ReadAsStringAsync(), options);
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

        return getTokenResponse;
    }
}
