namespace TennisScoreboard.Models;

public class MatchScoreModelBuilder(int firstPlayerId, int secondPlayerId)
{
    private readonly MatchScoreModel _matchScoreModel = new(firstPlayerId, secondPlayerId);

    public MatchScoreModelBuilder WithPointsInCurrentGame(string pointsString) {
        var points = pointsString.Split(':').Select(p => p switch {
            "0" => 0,
            "15" => 1,
            "30" => 2,
            "40" => 3,
            "AD" => 4,
            _ => throw new ArgumentException("Invalid points string")
        }).ToList();

        _matchScoreModel.FirstPlayerScores.PointsInCurrentGame = points[0];
        _matchScoreModel.SecondPlayerScores.PointsInCurrentGame = points[1];

        return this;
    }

    public MatchScoreModelBuilder WithTieBreak(string tieBreakPointsString) {
        var points = tieBreakPointsString.Split(':').Select(int.Parse).ToList();

        _matchScoreModel.FirstPlayerScores.PointsInCurrentGame = points[0];
        _matchScoreModel.SecondPlayerScores.PointsInCurrentGame = points[1];

        return WithGamesInCurrentSet("6:6");
    }

    public MatchScoreModelBuilder WithGamesInCurrentSet(string gamesString) {
        var games = gamesString.Split(':').Select(int.Parse).ToList();

        _matchScoreModel.FirstPlayerScores.GamesInCurrentSet = games[0];
        _matchScoreModel.SecondPlayerScores.GamesInCurrentSet = games[1];

        return this;
    }

    public MatchScoreModelBuilder WithFinishedSets(List<Tuple<int, int>> finishedSets) {
        var firstPlayerFinishedSets = finishedSets.Select(set => set.Item1).ToList();
        var secondPlayerFinishedSets = finishedSets.Select(set => set.Item2).ToList();
        
        _matchScoreModel.FirstPlayerScores.FinishedSets = firstPlayerFinishedSets;
        _matchScoreModel.SecondPlayerScores.FinishedSets = secondPlayerFinishedSets;

        return this;
    }

    public MatchScoreModel Build() => _matchScoreModel;
}