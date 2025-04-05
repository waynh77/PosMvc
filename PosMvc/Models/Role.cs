using Microsoft.AspNetCore.Identity;

namespace PosMvc.Models
{
    public class Role:IdentityRole
    {
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Menu>? Menus { get; set; }   
    }
}
