using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class FinishedMatchesArchiveService
{
    private readonly TennisMatchesContext context;

    public FinishedMatchesArchiveService(TennisMatchesContext context)
    {
        this.context = context;
    }

    public void ArchiveMatch(Match match, int winnerId)
    {
        match.WinnerId = winnerId;
        var matchToAdd = new Match
        {
            FirstPlayerId = match.FirstPlayerId,
            SecondPlayerId = match.SecondPlayerId,
            WinnerId = match.WinnerId
        };
        
        context.Matches.Add(matchToAdd);
        context.SaveChanges();
    }
}