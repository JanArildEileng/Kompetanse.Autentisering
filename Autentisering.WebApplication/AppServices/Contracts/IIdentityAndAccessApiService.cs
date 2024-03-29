﻿using Authorization.Shared.Dto.IdentityAndAccess;

namespace Authorization.WebBFFApplication.AppServices.Contracts
{
    public interface IIdentityAndAccessApiService
    {
        Task<string> Login(string userName = "TestUSer", string password = "Password");

        Task<string> GetAuthorizationCode(string client_id, string userName, string password);


        Task<GetTokenResponse> GetToken(string authorizationCode);
        Task<GetTokenResponse> GetRefreshedTokens(string refreshToken);


        Task<string> GetUserinfo(string AccessToken);

    }
}