using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Extentions;
using Application.Services.Authen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route($"api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpPost(ApiRoutes.Identity.CreateAccount)]
        public async Task<ActionResult<GeneralResponse>> CreateAccountAsync(CreateAccountRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.CreateAccountAsync(model));
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<ActionResult<GeneralResponse>> LoginAccountAsync(LoginRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.LoginAccountAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.RefreshToken)]
        public async Task<ActionResult<GeneralResponse>> RefreshTokenAsync(RefreshTokenRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.RefreshTokenAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.CreateRole)]
        public async Task<ActionResult<GeneralResponse>> CreateRoleAsync(CreateRoleRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.CreateRoleAsysnc(model));
        }

        [HttpGet(ApiRoutes.Identity.RoleList)]
        public async Task<ActionResult<IEnumerable<GeneralResponse>>> GetRoleAsync()
        {
            return Ok(await account.GetRolesAsync());
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.CreateSuperAdminAccount)]
        public async Task<ActionResult<GeneralResponse>> CreateSupperAdminAsync()
        {
            return Ok(await account.CreateSuperAdminAsync());
        }

        [HttpGet(ApiRoutes.Identity.UserWithRole)]
        public async Task<ActionResult<List<GeneralResponse>>> GetUserWithRoleAsync()
        {
            return Ok(await account.GetUsersWithRolesAsync());
        }

        [HttpPost(ApiRoutes.Identity.ChangePassword)]
        public async Task<ActionResult<GeneralResponse>> ChangePassAsync(ChangePassRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.ChangePassAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.ChangeUserRole)]
        public async Task<ActionResult<GeneralResponse>> ChangeRoleAsync(AssignUserRoleRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.ChangeUserRoleAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.AssignUserRole)]
        public async Task<ActionResult<GeneralResponse>> AssignUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.AssignUserRoleAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.DeleteUser)]
        public async Task<ActionResult<GeneralResponse>> DeleteAccountAsync(UpdateDeleteRequest model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.DeleteUserAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.DeleteUserRole)]
        public async Task<ActionResult<GeneralResponse>> DeleteUserAsync(AssignUserRoleRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.DeleteUserRoleAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.UpdateRole)]
        public async Task<ActionResult<GeneralResponse>> UpdateRoleAsync(UpdateRoleNameRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.UpdateRoleAsync(model));
        }
    }
}
