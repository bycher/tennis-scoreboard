using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class FinishedMatchesArchiveService(TennisMatchesContext context)
{
    private readonly TennisMatchesContext _context = context;

    public async Task ArchiveMatch(Match match, int winnerId)
    {
        var matchToAdd = new Match
        {
            FirstPlayerId = match.FirstPlayerId,
            SecondPlayerId = match.SecondPlayerId,
            WinnerId = winnerId
        };
        
        await _context.Matches.AddAsync(matchToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MatchesHistoryRecord>> GetFilteredMatchHistoryRecords(string? filterByPlayerName)
    {
        var matches = await _context.Matches
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .Include(m => m.Winner)
            .ToListAsync();
        var filter = new MatchesHistoryFilter(filterByPlayerName);

        return matches.Select(m => new MatchesHistoryRecord
            {
                FirstPlayerName = m.FirstPlayer.Name,
                SecondPlayerName = m.SecondPlayer.Name,
                WinnerName = m.Winner.Name
            })
            .Where(mhr => mhr.IsFitUnderFilter(filter));
    }
}