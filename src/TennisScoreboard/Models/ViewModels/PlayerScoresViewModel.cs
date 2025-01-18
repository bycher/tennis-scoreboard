using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Models.ViewModels;

public class PlayerScoreViewModel(MatchScoreDto matchScore, Player player) {
    public MatchScoreDto MatchScore { get; set; } = matchScore;
    public Player Player { get; set; } = player;

    public string GetPointsAsString() => GetComponent<string>(
        nameof(PlayerScoresDto.PointsAsString), [MatchScore.IsTieBreak]
    );

    public int GetGames() => GetComponent<int>(nameof(PlayerScoresDto.Games));

    public List<int> GetSets() => GetComponent<List<int>>(nameof(PlayerScoresDto.Sets));

    private T GetComponent<T>(string componentName, object?[]? parameters = null) {
        MatchScore.TryGetScoreComponent(componentName, Player.Id, out T? component, parameters);
        return component ?? throw new InvalidOperationException(GetScoreComponentFailedMessage(componentName));
    }

    private static string GetScoreComponentFailedMessage(string componentName) =>
        $"Failed to get '{componentName}' for player";
}