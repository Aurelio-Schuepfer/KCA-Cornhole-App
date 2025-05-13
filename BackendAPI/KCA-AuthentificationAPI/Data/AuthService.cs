using KCA_AuthentificationAPI.Data;
using KCA_AuthentificationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<AppUser> _passwordHasher;

    public AuthService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<AppUser>();
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        var exists = await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
        if (exists) return false;

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AppUser?> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success ? user : null;
    }
}
