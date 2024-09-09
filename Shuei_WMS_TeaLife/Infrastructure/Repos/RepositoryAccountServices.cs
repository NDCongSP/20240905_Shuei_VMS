using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extentions;
using Application.Services.Authen;
using Azure;
using Domain.Entity.Authentication;
using Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
//using static Application.Extentions.Constant;

namespace Infrastructure.Repos
{
    public class RepositoryAccountServices(RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager, IConfiguration config,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context) : IAccount
    {
        #region Methods
        private async Task<ApplicationUser> FindUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);
        private async Task<ApplicationUser> FindUserByNameAsync(string name)
            => await userManager.FindByNameAsync(name);
        private async Task<IdentityRole> FindRoleByNameAsync(string roleName)
            => await roleManager.FindByNameAsync(roleName);

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<GeneralResponse> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            if (user == null || role is null)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = "Model state cannot be empty"
                };

            if (await FindRoleByNameAsync(role.Name) == null) await CreateRoleAsysnc(role.Adapt(new CreateRoleRequestDTO()));

            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
            string error = CheckReponse(result);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = error
                };
            else
                return new GeneralResponse()
                {
                    Flag = true,
                    Message = $"{user.Name} assigned to {role.Name} role"
                };
        }

        private static string CheckReponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(_ => _.Description);
                return string.Join(Environment.NewLine, error);
            }

            return null;
        }

        private async Task<string> GenerateToken1(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,(await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("FullName",user.Name)
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(double.TryParse(config["JwtExpiryTime"], out double value) ? value : 30),
                    signingCredentials: credentials
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null;
            }
        }
        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddMinutes(double.TryParse(config["JwtExpiryTime"], out double value) ? value : 60);

            var userClaims = new[]
               {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,(await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("FullName",user.Name)
                };

            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                expires: expiry,
                claims: userClaims,
                signingCredentials: credentials
                );

            return token;
        }

        private async Task<GeneralResponse> SaveRefreshTokenAsync(string userId, string token)
        {
            try
            {
                var user = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
                if (user == null)
                    await context.RefreshTokens.AddAsync(new RefreshToken() { UserId = userId, Token = token });
                else
                    user.Token = token;
                await context.SaveChangesAsync();
                return new GeneralResponse()
                {
                    Flag = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }
        #endregion


        public async Task<GeneralResponse> ChangeUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            var user = await FindUserByNameAsync(model.UserName);

            if (user == null)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = "User not found"
                };

            if (await FindRoleByNameAsync(model.RoleName) is null)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = "Role not found"
                };

            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);

            var error = CheckReponse(removeOldRole);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = error
                };

            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            var response = CheckReponse(result);
            if (!string.IsNullOrEmpty(response))
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = response
                };
            else
                return new GeneralResponse()
                {
                    Flag = true,
                    Message = "Role changed"
                };
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountRequestDTO model)
        {
            try
            {
                if (await userManager.FindByNameAsync(model.UserName) != null)
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = "Sorry, user is already created."
                    };

                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.Name,
                    Email = model.Email,
                    PasswordHash = model.Password
                };

                var result = await userManager.CreateAsync(user, model.Password);
                string error = CheckReponse(result);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = error
                    };

                var res = await AssignUserToRole(user, new IdentityRole() { Name = model.Role });
                return new GeneralResponse()
                {
                    Flag = res.Flag,
                    Message = res.Message
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                if ((await FindRoleByNameAsync(ConstantExtention.Role.Admin)) != null) return;
                var admin = new CreateAccountRequestDTO()
                {
                    Name = "Admin",
                    Password = "Admin123@456",
                    Email = "admin@gmail.com",
                    Role = ConstantExtention.Role.Admin
                };

                await CreateAccountAsync(admin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> CreateRoleAsysnc(CreateRoleRequestDTO model)
        {
            try
            {
                if (await FindRoleByNameAsync(model.Name) != null)
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = $"{model.Name} already created."
                    };

                var respone = await roleManager.CreateAsync(new IdentityRole() { Name = model.Name });
                var error = CheckReponse(respone);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = error
                    };
                else
                    return new GeneralResponse()
                    {
                        Flag = true,
                        Message = $"{model.Name} created."
                    };
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<IEnumerable<GetRoleResponseDTO>> GetRolesAsync()
            => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleResponseDTO>>();

        public async Task<IEnumerable<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers == null) return null;

            var list = new List<GetUserWithRoleResponseDTO>();
            foreach (var user in allUsers)
            {
                var getUserRole = (await userManager.GetRolesAsync(user)).ToList();
                var roles = new List<GetRoleResponseDTO>();

                foreach (var roleName in getUserRole)
                {
                    var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());

                    roles.Add(new GetRoleResponseDTO()
                    {
                        Id = getRoleInfo.Id,
                        Name = getRoleInfo.Name,
                    });
                }

                //var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == getUserRole.ToLower());
                list.Add(new GetUserWithRoleResponseDTO()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Roles = roles,
                    //RoleId = getRoleInfo.Id,
                    //RoleName = getRoleInfo.Name,
                });
            }
            return list;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginRequestDTO model)
        {
            try
            {
                //var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                //if(!result.Succeeded) return new LoginResponse() { flag = false ,message="Username and password are invalid."};

                var user = await FindUserByNameAsync(model.UserName);
                if (user == null) return new LoginResponse()
                {
                    Flag = false,
                    Message = "User not found",
                };

                SignInResult result = null;
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch
                {
                    return new LoginResponse()
                    {
                        Flag = false,
                        Message = "Invalid credentials"
                    };
                }

                if (!result.Succeeded) return new LoginResponse()
                {
                    Flag = false,
                    Message = "Invalid credentials"
                };

                var jwtToken = await GenerateToken(user);
                string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                string refreshToken = GenerateRefreshToken();

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
                    return new LoginResponse()
                    {
                        Flag = false,
                        Message = "Error occured while in account, please contact administrator."
                    };
                else
                {
                    //save token after login successfull 
                    var saveResult = await SaveRefreshTokenAsync(user.Id, refreshToken);
                    if (saveResult.Flag)
                        return new LoginResponse()
                        {
                            Flag = true,
                            Message = $"{user.Name} successfully logged in.",
                            Token = token,
                            RefreshToken = refreshToken,
                            Expiration = jwtToken.ValidTo.ToString()
                        };
                    else return new LoginResponse();
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Flag = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequestDTO model)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.Token);
            if (token is null) return new LoginResponse();

            var user = await userManager.FindByIdAsync(token.UserId);
            var newjwtToken = await GenerateToken(user);
            string newToken = new JwtSecurityTokenHandler().WriteToken(newjwtToken);
            string newRefreshToken = GenerateRefreshToken();

            var saveResult = await SaveRefreshTokenAsync(user.Id, newRefreshToken);
            if (saveResult.Flag)
                return new LoginResponse()
                {
                    Flag = true,
                    Message = $"{user.Name} successfully re-logged in.",
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    Expiration = newjwtToken.ValidTo.ToString()
                };
            else
                return new LoginResponse();
        }

        public async Task<GeneralResponse> ChangePassAsync(ChangePassRequestDTO model)
        {
            var user = await FindUserByEmailAsync(model.UserName);
            if (user == null)
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = "User not found"
                };

            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, model.NewPassword);

            return new GeneralResponse()
            {
                Flag = true,
                Message = "Passwork changed"
            };
        }

        public async Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleRequestDTO model)
        {
            try
            {
                if (model == null)
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = "Model state cannot be empty"
                    };

                var user = await FindUserByNameAsync(model.UserName);
                if (user == null)
                    return new GeneralResponse()
                    {
                        Flag = false,
                        Message = "User not found."
                    };

                var result = await AssignUserToRole(user, new IdentityRole() { Name = model.RoleName });
                return new GeneralResponse()
                {
                    Flag = result.Flag,
                    Message = result.Message
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }
    }
}
