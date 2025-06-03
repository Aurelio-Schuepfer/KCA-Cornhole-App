using Microsoft.AspNetCore.Identity;
using System;

namespace KCA_AuthentificationAPI.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime? LastUsernameChange { get; set; }
    }
}