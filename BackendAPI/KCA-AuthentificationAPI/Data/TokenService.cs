using KCA_AuthentificationAPI.Models;// Enthält die AppUser-Klasse, die von IdentityUser erbt
using Microsoft.IdentityModel.Tokens; //Erstellen von kryptografischen Token
using System.IdentityModel.Tokens.Jwt;// Erstellen und Verarbeiten von JWTs
using System.Security.Claims;
using System.Text;

namespace KCA_AuthentificationAPI.Data
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) // Konstruktor, der IConfiguration entgegennimmt
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user, bool RememberMe)
        {
            var SecretKey = _configuration["JwtSettings:SecretKey"]; // secretKey wird aus appsettings.json geholt
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)); // Erstellen eines symmetrischen Sicherheitsschlüssels aus dem geheimen Schlüssel
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Erstellen von Anmeldeinformationen mit dem Schlüssel und dem HMAC SHA-256-Algorithmus

            var claims = new[] // Erstellen von Daten, die im Token enthalten sein sollen
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expires = RememberMe // Setzen des Ablaufdatums basierend auf der RememberMe-Einstellung
            ? DateTime.UtcNow.AddDays(30) 
            : DateTime.UtcNow.AddHours(1); 

            var token = new JwtSecurityToken( // Erstellen des JWT-Tokens
                issuer: null,
                audience: null,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
