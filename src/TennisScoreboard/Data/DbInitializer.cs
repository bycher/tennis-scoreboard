using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Models;

namespace TennisScoreboard.Data;

public class DbInitializer(TennisMatchesContext context) {
    private readonly List<Player> _players = [
        new Player { Name = "Roger Federer" },
        new Player { Name = "Rafael Nadal" },
        new Player { Name = "Novak Djokovic" },
        new Player { Name = "Andy Murray" },
        new Player { Name = "Stan Wawrinka" },
        new Player { Name = "Marin Cilic" },
        new Player { Name = "Dominic Thiem" },
        new Player { Name = "Alexander Zverev" },
        new Player { Name = "Stefanos Tsitsipas" },
        new Player { Name = "Daniil Medvedev" }
    ];

    private readonly Random _rand = new();

    public async Task InitializeAsync() {
        await context.Database.EnsureCreatedAsync();        
        
        await InitializePlayers();
        await InitializeMatches();
    }

    private async Task InitializePlayers() {
        if (await context.Players.AnyAsync())
            return;

        await context.Players.AddRangeAsync(_players);
        await context.SaveChangesAsync();
    }

    private async Task InitializeMatches() {
        if (await context.Matches.AnyAsync())
            return;
        
        var playerIds = await context.Players.Select(p => p.Id).ToListAsync();

        // Seed with all pairs of players from _players and random winner
        for (var firstPlayerId = 1; firstPlayerId < playerIds.Count; firstPlayerId++)
            for (var secondPlayerId = firstPlayerId + 1; secondPlayerId < playerIds.Count + 1; secondPlayerId++) {
                var firstPlayer = context.Players.Find(firstPlayerId) ??
                    throw new InvalidOperationException("Player not found");
                var secondPlayer = context.Players.Find(secondPlayerId) ??
                    throw new InvalidOperationException("Player not found");
                
                var match = new Match {
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId,
                    WinnerId = GetRandomWinner(firstPlayerId, secondPlayerId),
                    FirstPlayer = firstPlayer,
                    SecondPlayer = secondPlayer
                };

                await context.Matches.AddAsync(match);
                await context.SaveChangesAsync();
            }
    }

    private int GetRandomWinner(int firstPlayerId, int secondPlayerId) =>
        _rand.Next(2) == 0 ? firstPlayerId : secondPlayerId;
}