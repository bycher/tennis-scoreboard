using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Tests.Utils;

public class MatchScoreBuilder(int firstPlayerId, int secondPlayerId) {
    private readonly MatchScoreDto _matchScore = new(
        new Player { Id = firstPlayerId },
        new Player { Id = secondPlayerId }
    );

    public MatchScoreBuilder WithPoints(string firstPlayerPointsString, string secondPlayerPointsString) {
        var firstPlayerPoints = PointsAsInt(firstPlayerPointsString);
        var secondPlayerPoints = PointsAsInt(secondPlayerPointsString);

        _matchScore.SetScoreComponents(nameof(PlayerScoresDto.Points), firstPlayerPoints, secondPlayerPoints);

        return this;
    }

    public MatchScoreBuilder WithTieBreak(int firstPlayerPoints, int secondPlayerPoints) {
        _matchScore.SetScoreComponents(nameof(PlayerScoresDto.Points), firstPlayerPoints, secondPlayerPoints);

        return WithGames(6, 6);
    }

    public MatchScoreBuilder WithGames(int firstPlayerGames, int secondPlayerGames) {
        _matchScore.SetScoreComponents(nameof(PlayerScoresDto.Games), firstPlayerGames, secondPlayerGames);

        return this;
    }

    public MatchScoreBuilder WithSets(List<(int First, int Second)> Sets) {
        var firstPlayerSets = Sets.Select(set => set.First).ToList();
        var secondPlayerSets = Sets.Select(set => set.Second).ToList();

        _matchScore.SetScoreComponents(nameof(PlayerScoresDto.Sets), firstPlayerSets, secondPlayerSets);

        return this;
    }

    public MatchScoreDto Build() => _matchScore;

    private static int PointsAsInt(string pointsAsString) => pointsAsString switch {
        "0" => 0,
        "15" => 1,
        "30" => 2,
        "40" => 3,
        "AD" => 4,
        _ => throw new ArgumentException("Invalid points string")
    };
}