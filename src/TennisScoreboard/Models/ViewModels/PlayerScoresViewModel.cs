using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Models.ViewModels;

public class PlayerScoreViewModel
{
    public required MatchScoreDto MatchScore { get; set; }
    public required Player Player { get; set; }

    public string GetPointsAsString()
    {
        MatchScore.TryGetScoreComponent(
                Player.Id,
                nameof(PlayerScoresDto.PointsAsString),
                out string? pointsAsString,
                [MatchScore.IsTieBreak]);

        return pointsAsString ?? throw new InvalidOperationException("Can't get points for player");
    }

    public int GetGames()
    {
        MatchScore.TryGetScoreComponent(Player.Id, nameof(PlayerScoresDto.Games), out int? games);
        return games ?? throw new InvalidOperationException("Can't get games for player");
    }

    public List<int> GetSets()
    {
        MatchScore.TryGetScoreComponent(Player.Id, nameof(PlayerScoresDto.Sets), out List<int>? sets);
        return sets ?? throw new InvalidOperationException("Can't get sets for player");
    }
}