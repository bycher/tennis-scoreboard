namespace TennisScoreboard.Models;

public class MatchScoreModelBuilder(int firstPlayerId, int secondPlayerId)
{
    private readonly MatchScoreModel _matchScoreModel = new(firstPlayerId, secondPlayerId);

    public MatchScoreModelBuilder WithPoints(string pointsString) {
        var points = pointsString.Split(':').Select(p => p switch {
            "0" => 0,
            "15" => 1,
            "30" => 2,
            "40" => 3,
            "AD" => 4,
            _ => throw new ArgumentException("Invalid points string")
        }).ToList();

        _matchScoreModel.FirstPlayerScores.Points = points[0];
        _matchScoreModel.SecondPlayerScores.Points = points[1];

        return this;
    }

    public MatchScoreModelBuilder WithTieBreak(string tieBreakPointsString) {
        var points = tieBreakPointsString.Split(':').Select(int.Parse).ToList();

        _matchScoreModel.FirstPlayerScores.Points = points[0];
        _matchScoreModel.SecondPlayerScores.Points = points[1];

        return WithGames("6:6");
    }

    public MatchScoreModelBuilder WithGames(string gamesString) {
        var games = gamesString.Split(':').Select(int.Parse).ToList();

        _matchScoreModel.FirstPlayerScores.Games = games[0];
        _matchScoreModel.SecondPlayerScores.Games = games[1];

        return this;
    }

    public MatchScoreModelBuilder WithSets(List<Tuple<int, int>> Sets) {
        var firstPlayerSets = Sets.Select(set => set.Item1).ToList();
        var secondPlayerSets = Sets.Select(set => set.Item2).ToList();
        
        _matchScoreModel.FirstPlayerScores.Sets = firstPlayerSets;
        _matchScoreModel.SecondPlayerScores.Sets = secondPlayerSets;

        return this;
    }

    public MatchScoreModel Build() => _matchScoreModel;
}