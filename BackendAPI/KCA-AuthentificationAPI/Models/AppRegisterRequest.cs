//Registrierungsmodell für das Frontend
using System.ComponentModel.DataAnnotations.Schema;

namespace KCA_AuthentificationAPI.Models
{
    public class AppRegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        [Column(TypeName = "longtext")]
        public string Password { get; set; }
    }
}
