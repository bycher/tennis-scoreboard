using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Services;

public class MatchesHistoryService(TennisMatchesContext dbContext) {
    public async Task AddToHistory(MatchScoreUpdateContextDto scoreContext) {
        var matchToAdd = new Match {
            FirstPlayerId = scoreContext.MatchScore.FirstPlayer.Id,
            SecondPlayerId = scoreContext.MatchScore.SecondPlayer.Id,
            WinnerId = scoreContext.WinnerId
        };
        
        await dbContext.Matches.AddAsync(matchToAdd);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MatchesHistoryRecordDto>> GetFilteredHistory(
        MatchesHistoryFilterDto filter
    ) {
        var matches = await dbContext.Matches.Include(m => m.FirstPlayer)
                                             .Include(m => m.SecondPlayer)
                                             .Include(m => m.Winner)
                                             .ToListAsync();

        return matches.Select(m => new MatchesHistoryRecordDto {
                FirstPlayerName = m.FirstPlayer.Name,
                SecondPlayerName = m.SecondPlayer.Name,
                WinnerName = m.Winner.Name
            })
            .Where(mhr => mhr.IsFitUnderFilter(filter));
    }
}