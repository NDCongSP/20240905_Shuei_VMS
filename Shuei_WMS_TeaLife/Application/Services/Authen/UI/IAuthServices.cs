using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extentions;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authen.UI
{
    [BaseAddress(ApiRoutes.Identity.BasePath)]
    public interface IAuthServices
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

        //Task<string> TryRefreshTokenAsync(RefreshTokenRequestDTO model);
        Task LogoutAsync();
        Task<GeneralResponse> DeleteUserAsync(UpdateDeleteRequestDTO model);
        Task<GeneralResponse> UpdateRoleAsync(UpdateRoleNameRequestDTO model);
    }
}
