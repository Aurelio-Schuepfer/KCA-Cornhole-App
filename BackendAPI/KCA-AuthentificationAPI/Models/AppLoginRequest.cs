//Loginmodell für das Frontend
using System.ComponentModel.DataAnnotations.Schema;

namespace KCA_AuthentificationAPI.Models
{
    public class AppLoginRequest
    {
        public string UserName { get; set; }
        [Column(TypeName = "longtext")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
