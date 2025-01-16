using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Models.ViewModels;

public class MatchScoreViewModel
{
    public required MatchScoreDto MatchScore { get; set; }
    public Guid Uuid { get; set; }
    public int WinnerId { get; set; }

    public string WinnerName => MatchScore.FirstPlayer.Id == WinnerId
        ? MatchScore.FirstPlayer.Name
        : MatchScore.SecondPlayer.Name;
}