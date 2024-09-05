using Microsoft.AspNetCore.Identity;

namespace myshop.Entities.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }=string.Empty;
        public string Address { get; set; }=string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
