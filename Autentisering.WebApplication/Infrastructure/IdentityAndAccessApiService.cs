using Authorization.RefitApi;
using Authorization.Shared.Dto.IdentityAndAccess;
using Authorization.WebBFFApplication.AppServices.Contracts;
using Refit;
using System.Text.Json;

namespace Authorization.WebBFFApplication.Infrastructure;

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
        string? identityName;
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
        string? authorizationCode;
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
        string? userinfo;
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
        GetTokenResponse getTokenResponse;
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

    public async Task<GetTokenResponse> GetTokenCodeFlow(string grant_type = "authorization_code",
                                                         string code = "B6E4-B7B966BA3A89",
                                                         string redirect_uri = "",
                                                         string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89",
                                                         string client_secret = "secret_123")
    {
        GetTokenResponse getTokenResponse;
        try
        {
            var response = await identityApi.GetTokenCodeFlow(code);
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
        GetTokenResponse getTokenResponse;
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
