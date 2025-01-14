using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class PlayersService
{
    private readonly TennisMatchesContext _context;

    public PlayersService(TennisMatchesContext context)
    {
        _context = context;
    }

    public async Task<Player> AddPlayer(string name)
    {
        if (_context.Players.Any(p => p.Name == name))
            return _context.Players.Single(p => p.Name == name);

        var player = await _context.Players.AddAsync(new Player { Name = name });
        await _context.SaveChangesAsync();

        return player.Entity;
    }
}