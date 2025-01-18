namespace TennisScoreboard.Models.Dtos;

public class MatchScoreUpdateContextDto(MatchScoreDto matchScore, int winnerId) {
    public MatchScoreDto MatchScore { get; set; } = matchScore;
    public int WinnerId { get; set; } = winnerId;

    public bool IsValid => WinnerId == MatchScore.FirstPlayer.Id ||
                           WinnerId == MatchScore.SecondPlayer.Id;
}