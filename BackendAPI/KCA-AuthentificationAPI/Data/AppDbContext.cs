using KCA_AuthentificationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KCA_AuthentificationAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users { get; set; }
}
