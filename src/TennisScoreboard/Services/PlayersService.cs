using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Services;

public class PlayersService(TennisMatchesContext context)
{
    private readonly TennisMatchesContext _context = context;

    public async Task<(Player, Player)> AddPlayers(string firstPlayerName, string secondPlayerName)
    {
        var firstPlayer = await AddPlayer(firstPlayerName);
        var secondPlayer = await AddPlayer(secondPlayerName);

        return (firstPlayer, secondPlayer);
    }

    public async Task<Player> AddPlayer(string playerName)
    {
        var player = new Player { Name = playerName };

        try
        {
            var playerEntry = await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            return playerEntry.Entity;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintVioldated(ex))
        {
            _context.Entry(player).State = EntityState.Detached;
            return await _context.Players.SingleAsync(p => p.Name == playerName);
        }
    }

    private static bool IsUniqueConstraintVioldated(DbUpdateException ex) => 
        ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19;
}