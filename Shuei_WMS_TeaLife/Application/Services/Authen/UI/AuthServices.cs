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
using System.Reflection;
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

        public async Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.AssignUserRole}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<GeneralResponse> ChangePassAsync(ChangePassRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.ChangePassword}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response!;
        }

        public async Task<GeneralResponse> ChangeUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.ChangeUserRole}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.CreateAccount}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<GeneralResponse> CreateSuperAdminAsync()
        {
            var result = await _httpClient.PostAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.CreateSuperAdminAccount}",null);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<GeneralResponse> CreateRoleAsysnc(CreateRoleRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.CreateRole}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<GeneralResponse> DeleteUserAsync(string userName)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.DeleteUser}", userName);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }

        public async Task<List<GetRoleResponseDTO>> GetRolesAsync()
        {
            var result = await _httpClient.GetAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.RoleList}");

            if (!result.IsSuccessStatusCode)
                return new List<GetRoleResponseDTO>();

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<List<GetRoleResponseDTO>>(content);

            return response;
        }

        public async Task<List<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync()
        {
            var result = await _httpClient.GetAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.UserWithRole}");

            if (!result.IsSuccessStatusCode)
                return new List<GetUserWithRoleResponseDTO>();

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<List<GetUserWithRoleResponseDTO>>(content);
            return response;
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
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated();
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

        public async Task<GeneralResponse> UpdateRoleAsync(UpdateRoleNameRequestDTO model)
        {
            var result = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Identity.BasePath}/{ApiRoutes.Identity.UpdateRole}", model);

            if (!result.IsSuccessStatusCode)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = result.StatusCode.ToString()
                };

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GeneralResponse>(content);

            return response;
        }
    }
}
