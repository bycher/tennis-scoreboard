using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class FinishedMatchesArchiveService(TennisMatchesContext context) {
    public void ArchiveMatch(MatchScoreModel match) {
        context.Matches.Add(match.Match);
        context.SaveChanges();
    }
}