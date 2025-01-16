using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Services;

public class MatchesHistoryService(TennisMatchesContext context)
{
    private readonly TennisMatchesContext _context = context;

    public async Task AddToHistory(MatchScoreUpdateContextDto context)
    {
        var matchToAdd = new Match
        {
            FirstPlayerId = context.MatchScore.FirstPlayer.Id,
            SecondPlayerId = context.MatchScore.SecondPlayer.Id,
            WinnerId = context.WinnerId
        };
        
        await _context.Matches.AddAsync(matchToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MatchesHistoryRecordDto>> GetFilteredHistory(MatchesHistoryFilterDto filter)
    {
        var matches = await _context.Matches
            .Include(m => m.FirstPlayer).Include(m => m.SecondPlayer).Include(m => m.Winner)
            .ToListAsync();

        return matches.Select(m => new MatchesHistoryRecordDto
            {
                FirstPlayerName = m.FirstPlayer.Name,
                SecondPlayerName = m.SecondPlayer.Name,
                WinnerName = m.Winner.Name
            })
            .Where(mhr => mhr.IsFitUnderFilter(filter));
    }
}