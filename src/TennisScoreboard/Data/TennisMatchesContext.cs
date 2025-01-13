using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Models;

namespace TennisScoreboard.Data;

public class TennisMatchesContext : DbContext
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;

    public TennisMatchesContext(DbContextOptions<TennisMatchesContext> options) : base(options)
    {
        Database.OpenConnection();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>()
            .HasOne(m => m.FirstPlayer)
            .WithMany(p => p.Matches)
            .HasForeignKey(m => m.FirstPlayerId);
        
        modelBuilder.Entity<Match>()
            .HasOne(m => m.SecondPlayer)
            .WithMany()
            .HasForeignKey(m => m.SecondPlayerId);
    }
}