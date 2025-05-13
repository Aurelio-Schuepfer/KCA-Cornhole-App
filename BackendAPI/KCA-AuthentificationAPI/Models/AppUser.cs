using Microsoft.AspNetCore.Identity;
namespace KCA_AuthentificationAPI.Models
{
    public class AppUser : IdentityUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
