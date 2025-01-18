using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Services;

public class PlayersService(TennisMatchesContext context) {
    public async Task<Player?> GetPlayerById(int playerId) => await context.Players.FindAsync(playerId);

    public async Task<(Player, Player)> AddPlayers(string firstPlayerName, string secondPlayerName) {
        var firstPlayer = await AddPlayer(firstPlayerName);
        var secondPlayer = await AddPlayer(secondPlayerName);

        return (firstPlayer, secondPlayer);
    }

    public async Task<Player> AddPlayer(string playerName) {
        var player = new Player { Name = playerName };

        try {
            var playerEntry = await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
            return playerEntry.Entity;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintVioldated(ex)) {
            context.Entry(player).State = EntityState.Detached;
            return await context.Players.SingleAsync(p => p.Name == playerName);
        }
    }

    private static bool IsUniqueConstraintVioldated(DbUpdateException ex) => 
        ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19;
}