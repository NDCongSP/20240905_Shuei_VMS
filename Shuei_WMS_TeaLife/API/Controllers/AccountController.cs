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

        [HttpPost(ApiRoutes.Identity.RoleCreate)]
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

        [HttpPost(ApiRoutes.Identity.CreateAdminAccount)]
        public async Task<ActionResult> CreateAdminAsync()
        {
            await account.CreateAdmin();
            return Ok();
        }

        [HttpGet(ApiRoutes.Identity.UserWithRole)]
        public async Task<ActionResult<IEnumerable<GeneralResponse>>> GetUserWithRoleAsync()
        {
            return Ok(await account.GetUsersWithRolesAsync());
        }

        [HttpPost(ApiRoutes.Identity.ChangePassword)]
        public async Task<ActionResult<GeneralResponse>> ChangePassAsync(ChangePassRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.ChangePassAsync(model));
        }

        [HttpPost(ApiRoutes.Identity.ChangrRole)]
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
    }
}
