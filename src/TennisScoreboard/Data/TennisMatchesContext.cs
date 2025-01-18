using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Data;

public class TennisMatchesContext : DbContext {
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;

    public TennisMatchesContext(DbContextOptions<TennisMatchesContext> options) : base(options) {
        Database.OpenConnection();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Match>()
                    .HasOne(m => m.FirstPlayer)
                    .WithMany()
                    .HasForeignKey(m => m.FirstPlayerId);
        
        modelBuilder.Entity<Match>()
                    .HasOne(m => m.SecondPlayer)
                    .WithMany()
                    .HasForeignKey(m => m.SecondPlayerId);
        
        modelBuilder.Entity<Match>()
                    .HasOne(m => m.Winner)
                    .WithMany()
                    .HasForeignKey(m => m.WinnerId);
    }
}