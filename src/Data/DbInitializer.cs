using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Models;

namespace TennisScoreboard.Data;

public static class DbInitializer {
    public static async Task InitializeAsync(TennisMatchesContext context) {
        await context.Database.EnsureCreatedAsync();

        if (await context.Players.AnyAsync())
            return;

        var players = new List<Player>() {
            new() { Name = "Roger Federer" },
            new() { Name = "Rafael Nadal" },
            new() { Name = "Novak Djokovic" },
            new() { Name = "Andy Murray" },
            new() { Name = "Stan Wawrinka" },
            new() { Name = "Marin Cilic" },
            new() { Name = "Dominic Thiem" },
            new() { Name = "Alexander Zverev" },
            new() { Name = "Stefanos Tsitsipas" },
            new() { Name = "Daniil Medvedev" }
        };

        await context.Players.AddRangeAsync(players);
        await context.SaveChangesAsync();
    }
}