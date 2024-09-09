using Microsoft.AspNetCore.Identity;

namespace Domain.Entity.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? AppName { get; set; }
    }
}
