using KCA_AuthentificationAPI.Models;// Enthält die AppUser-Klasse, die von IdentityUser erbt
using Microsoft.IdentityModel.Tokens; //Erstellen von kryptografischen Token
using System.IdentityModel.Tokens.Jwt;// Erstellen und Verarbeiten von JWTs
using System.Security.Claims;// Erstellen und Verarbeiten von Ansprüchen
using System.Text;// Kodierung und Dekodierung von Zeichenfolgen

namespace KCA_AuthentificationAPI.Data
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) // Konstruktor, der IConfiguration entgegennimmt
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user)
        {
            var secretKey = user.PasswordHash; // Hier wird der PasswordHash des Benutzers als geheimer Schlüssel verwendet
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Erstellen eines symmetrischen Sicherheitsschlüssels aus dem geheimen Schlüssel
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Erstellen von Anmeldeinformationen mit dem Schlüssel und dem HMAC SHA-256-Algorithmus

            var claims = new[] // Erstellen von Ansprüchen, die im Token enthalten sein sollen
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken( // Erstellen des JWT-Tokens
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
