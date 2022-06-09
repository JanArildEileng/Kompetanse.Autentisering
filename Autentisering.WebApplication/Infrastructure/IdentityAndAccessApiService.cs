using Autentisering.RefitApi;
using Autentisering.Shared.IdentityAndAccess;
using Autentisering.WebApplication.AppServices.Contracts;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http.Json;
using System.Text.Json;

namespace Autentisering.WebApplication.Infrastructure;

public class IdentityAndAccessApiService : IIdentityAndAccessApiService
{
    private readonly ILogger<IdentityAndAccessApiService> logger;
    private readonly IIdentityAndAccessApi identityApi;

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };


    public IdentityAndAccessApiService(ILogger<IdentityAndAccessApiService> logger, IIdentityAndAccessApi identityApi)
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
        GetTokenResponse getTokenResponse = null;

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



    public async Task<GetTokenResponse> GetRefreshedTokens(string refreshToken)
    {
        GetTokenResponse getTokenResponse = null;

        try
        {
            var response = await identityApi.GetRefreshedTokens(refreshToken);
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
