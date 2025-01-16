using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Models.ViewModels;

public class MatchScoreViewModel(MatchScoreDto matchScore, Guid uuid)
{
    public MatchScoreDto MatchScore { get; set; } = matchScore;
    public Guid Uuid { get; set; } = uuid;
}