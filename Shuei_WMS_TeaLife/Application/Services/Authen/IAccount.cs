using System;
using System.Collections.Generic;
using System.Linq;
using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Application.DTOs.Response;

namespace Application.Services.Authen
{
    public interface IAccount
    {
        Task<GeneralResponse> CreateSuperAdminAsync();
        Task<GeneralResponse> CreateAccountAsync(CreateAccountRequestDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginRequestDTO model);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequestDTO model);
        Task<GeneralResponse> CreateRoleAsysnc(CreateRoleRequestDTO model);
        Task<List<GetRoleResponseDTO>> GetRolesAsync();
        Task<List<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync();
        Task<GeneralResponse> ChangeUserRoleAsync(AssignUserRoleRequestDTO model);
        Task<GeneralResponse> ChangePassAsync(ChangePassRequestDTO model);
        Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleRequestDTO model);
        Task<GeneralResponse> DeleteUserAsync(UpdateDeleteRequest model);

        Task<GeneralResponse> DeleteUserRoleAsync(AssignUserRoleRequestDTO model);
        Task<GeneralResponse> UpdateRoleAsync(UpdateRoleNameRequestDTO model);
    }
}
