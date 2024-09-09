using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extentions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Application.Extentions.ConstantExtention;

namespace Application.Services.Authen.UI
{
    public class AuthServices : IAuthServices
    {
        readonly HttpClient _httpClient;
        readonly ILocalStorageService _localStorage;
        readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthServices(HttpClient httpClient, ILocalStorageService localStorage, ApiAuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> ChangePassAsync(ChangePassRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> ChangeUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> CreateAccountAsync(CreateAccountRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task CreateAdmin()
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> CreateRoleAsysnc(CreateRoleRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetRoleResponseDTO>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.Login}", model);

            if (!result.IsSuccessStatusCode)
                return new LoginResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);

            //Set local storage
            await _authStateProvider.CacheAuthTokensAsync(loginResponse.Token, loginResponse.RefreshToken, string.Empty);
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(model.UserName);
            //Gán token này mặc đinh vào header của tất cả các request của httpClient có tên là Bearer
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);

            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            await _authStateProvider.ClearCacheAsync();
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequestDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
