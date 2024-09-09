using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Account
{
    public class CreateAccountRequestDTO : LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required] public string Email { get; set; }
        [Required] public string FullName { get; set; } = string.Empty;

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public List<CreateRoleRequestDTO> Roles { get; set; }
    }
}
