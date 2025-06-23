using KCA_TournamentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KCA_TournamentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<Sponsor> Sponsors { get; set; } = null!;
        public DbSet<Partner> Partners { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<ParticipantStats> PlayerStats { get; set; } = null!;
        public DbSet<TeamStats> TeamStats { get; set; } = null!;
        public DbSet<TournamentStats> TournamentStats { get; set; } = null!;
        public DbSet<Bracket> Brackets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Tournaments)
                .WithMany(t => t.Teams);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamA)
                .WithMany()
                .HasForeignKey(m => m.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamB)
                .WithMany()
                .HasForeignKey(m => m.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tournament>()
            .HasMany(t => t.Participants)
            .WithOne(p => p.Tournament)
            .HasForeignKey(p => p.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Stats)
                .WithOne(s => s.Team)
                .HasForeignKey<TeamStats>(s => s.TeamId);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Stats)
                .WithOne(s => s.Tournament)
                .HasForeignKey<TournamentStats>(s => s.TournamentId);

            modelBuilder.Entity<TournamentStats>()
                .HasOne(s => s.WinningTeam)
                .WithMany()
                .HasForeignKey(s => s.WinningTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Sponsors)
                .WithOne(s => s.Tournament)
                .HasForeignKey(s => s.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Partners)
                .WithOne(p => p.Tournament)
                .HasForeignKey(p => p.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}