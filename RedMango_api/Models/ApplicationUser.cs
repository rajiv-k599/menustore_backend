using Microsoft.AspNetCore.Identity;

namespace RedMango_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
