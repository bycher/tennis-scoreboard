using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Models.ViewModels;

public class MatchScoreViewModel(MatchScoreDto matchScore, Guid uuid, int? winnerId = null) {
    public MatchScoreDto MatchScore { get; set; } = matchScore;
    public Guid Uuid { get; set; } = uuid;
    
    public int? WinnerId { get; set; } = winnerId;

    public string WinnerName => MatchScore.FirstPlayer.Id == WinnerId
        ? MatchScore.FirstPlayer.Name
        : MatchScore.SecondPlayer.Name;
}