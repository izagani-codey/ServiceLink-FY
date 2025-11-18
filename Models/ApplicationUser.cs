using Microsoft.AspNetCore.Identity;

namespace ServiceLink.Models
{
    public class ApplicationUser : IdentityUser
    {
        // optional full name
        // Models/ApplicationUser.cs
        public string? FullName { get; set; }


        // role usually stored via IdentityUserRole, but keep a helper property if you used it
        public string? Role { get; set; }
    }
}
