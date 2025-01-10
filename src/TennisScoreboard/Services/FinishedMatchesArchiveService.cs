using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class FinishedMatchesArchiveService(TennisMatchesContext context) {
    public void ArchiveMatch(Match match, int winnerId) {
        match.WinnerId = winnerId;
        
        context.Matches.Add(new Match {
            FirstPlayerId = match.FirstPlayerId,
            SecondPlayerId = match.SecondPlayerId,
            WinnerId = match.WinnerId
        });
        context.SaveChanges();
    }
}