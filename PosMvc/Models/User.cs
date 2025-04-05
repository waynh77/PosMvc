using Microsoft.AspNetCore.Identity;

namespace PosMvc.Models
{
    public class User:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
