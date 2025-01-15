namespace TennisScoreboard.Models;

public class MatchScoreViewModel(MatchScore matchScore, Guid uuid)
{
    public MatchScore MatchScore { get; set; } = matchScore;
    public Guid Uuid { get; set; } = uuid;

    public string WinnerNameCss => MatchScore.WinnerName == MatchScore.Match.FirstPlayer.Name
        ? "first-player" : "second-player";
}