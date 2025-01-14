namespace TennisScoreboard.Models;

public class MatchScoreViewModel
{
    public MatchScore MatchScore { get; set; } = null!;
    public Guid Uuid { get; set; }

    public MatchScoreViewModel(MatchScore matchScore, Guid uuid)
    {
        MatchScore = matchScore;
        Uuid = uuid;
    }
}