using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Account
{
    public class CreateAccountRequestDTO : LoginRequestDTO
    {
        [Required]
        public string Name { get; set; }=string.Empty;
        [Required]public string Email { get; set; }

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
