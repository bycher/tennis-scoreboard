using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Tests.Utils;

public class MatchScoreBuilder(int firstPlayerId, int secondPlayerId)
{
    private readonly MatchScoreDto _MatchScore = new(firstPlayerId, secondPlayerId);

    public MatchScoreBuilder WithPoints(string pointsString)
    {
        var points = pointsString.Split(':').Select(PlayerScoresDto.PointsAsInt).ToList();

        _MatchScore.FirstPlayerScores.Points = points[0];
        _MatchScore.SecondPlayerScores.Points = points[1];

        return this;
    }

    public MatchScoreBuilder WithTieBreak(string tieBreakPointsString)
    {
        var points = tieBreakPointsString.Split(':').Select(int.Parse).ToList();

        _MatchScore.FirstPlayerScores.Points = points[0];
        _MatchScore.SecondPlayerScores.Points = points[1];

        return WithGames("6:6");
    }

    public MatchScoreBuilder WithGames(string gamesString)
    {
        var games = gamesString.Split(':').Select(int.Parse).ToList();

        _MatchScore.FirstPlayerScores.Games = games[0];
        _MatchScore.SecondPlayerScores.Games = games[1];

        return this;
    }

    public MatchScoreBuilder WithSets(List<Tuple<int, int>> Sets)
    {
        var firstPlayerSets = Sets.Select(set => set.Item1).ToList();
        var secondPlayerSets = Sets.Select(set => set.Item2).ToList();

        _MatchScore.FirstPlayerScores.Sets = firstPlayerSets;
        _MatchScore.SecondPlayerScores.Sets = secondPlayerSets;

        return this;
    }

    public MatchScoreDto Build() => _MatchScore;
}