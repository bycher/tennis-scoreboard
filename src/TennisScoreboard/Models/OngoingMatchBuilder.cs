namespace TennisScoreboard.Models;

public class OngoingMatchBuilder {
    private readonly OngoingMatch _match;

    public OngoingMatchBuilder(int firstPlayerId, int secondPlayerId) {
        _match = new OngoingMatch(firstPlayerId, secondPlayerId);
    }

    public OngoingMatchBuilder WithPointsInCurrentGame(int playerId, int points) {
        _match.PlayersMatchStates[playerId].PointsInCurrentGame = points;
        return this;
    }

    public OngoingMatchBuilder WithGamesInCurrentSet(int playerId, int games) {
        _match.PlayersMatchStates[playerId].GamesInCurrentSet = games;
        return this;
    }

    public OngoingMatchBuilder WithFinishedGames(int playerId, List<int> games) {
        _match.PlayersMatchStates[playerId].FinishedGames = games;
        return this;
    }

    public OngoingMatch Build() {
        return _match;
    }
}