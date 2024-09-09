using Application.Extentions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Application.Extentions.ConstantExtention;

namespace Application.Services.Authen.UI
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider, IAccessTokenProvider
    {
        //kiểm soát truy cập đồng thời vào tài nguyên trong các tình huống cần giới hạn số lượng tác vụ hoặc luồng có thể chạy cùng lúc,
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        readonly HttpClient _httpClient;
        readonly ILocalStorageService _localStorage;

        #region Public methods
        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        /// <summary>
        /// Get state and claim.
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var cachedToken = await GetCachedAuthTokenAsync();
            if (string.IsNullOrWhiteSpace(cachedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Generate claimsIdentity from cached token
            var claimsIdentity = new ClaimsIdentity(JwtHelper.GetClaimsFromJwt(cachedToken), "jwt");

            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimPrincipal);
        }

        public void MarkUserAsAuthenticated()
        {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(
            //        new[] { new Claim(ClaimTypes.Name, userName) }
            //        , ConstantExtention.StorageConst.AuthToken
            //        ));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState=Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
        public async ValueTask CacheAuthTokensAsync(string token, string refreshToken, string sessionId)
        {
            await _localStorage.SetItemAsync(ConstantExtention.StorageConst.AuthToken, token);
            //await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
            //await _localStorage.SetItemAsync(StorageConstants.Local.SessionId, sessionId);
        }

        public async ValueTask CachePermissionsAsync(ICollection<string> permissions) =>
           await _localStorage.SetItemAsync(ConstantExtention.StorageConst.Permission, permissions);

        public async Task ClearCacheAsync()
        {
            await _localStorage.RemoveItemAsync(ConstantExtention.StorageConst.AuthToken);
            //await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
            //await _localStorage.RemoveItemAsync(StorageConstants.Local.SessionId);
            //await _localStorage.RemoveItemAsync(StorageConstants.Local.Permissions);
        }

        public async ValueTask<AccessTokenResult> RequestAccessToken()
        {
            var authState = await GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated is not true)
            {
                return new AccessTokenResult(AccessTokenResultStatus.RequiresRedirect, null, "/login");
            }

            // We make sure the access token is only refreshed by one thread at a time. The other ones have to wait.
            await _semaphore.WaitAsync();
            try
            {
                string token = await GetCachedAuthTokenAsync();


                // Check if token needs to be refreshed (when its expiration time is less than 1 minute away)
                //var expTime = GetExpiration(authState.User);

                //var diff = expTime - DateTime.UtcNow;
                //if (diff.TotalSeconds <= 5)
                //{
                //    //string refreshToken = await GetCachedRefreshTokenAsync();
                //    //(bool succeeded, var response) = await TryRefreshTokenAsync(new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });
                //    //if (!succeeded)
                //    //{
                //    //    return new AccessTokenResult(AccessTokenResultStatus.RequiresRedirect, null, _authSettings.Value.LoginUrl);
                //    //}

                //    //token = response?.Token;
                //}
                //else if (diff.TotalSeconds < 0)
                //{
                //    return new AccessTokenResult(AccessTokenResultStatus.RequiresRedirect, new AccessToken() { Value = null }, "/login");
                //}

                return new AccessTokenResult(AccessTokenResultStatus.Success, new AccessToken() { Value = token }, string.Empty);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options) =>
           RequestAccessToken();
        #endregion

        #region Private Methods
        private async ValueTask<string> GetCachedAuthTokenAsync() =>
          await _localStorage.GetItemAsync<string>(ConstantExtention.StorageConst.AuthToken);

        private async ValueTask<string> GetCachedRefreshTokenAsync() =>
           await _localStorage.GetItemAsync<string>(ConstantExtention.StorageConst.RefreshToken);

        private async ValueTask<ICollection<string>> GetCachedPermissionsAsync() =>
          await _localStorage.GetItemAsync<ICollection<string>>(ConstantExtention.StorageConst.Permission);

        public static DateTimeOffset GetExpiration(ClaimsPrincipal principal)
        {
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(principal.FindFirst("exp")?.Value));
        }
        #endregion
    }
}
