namespace TennisScoreboard.Models;

public class OngoingMatch(int firstPlayerId, int secondPlayerId) {
    private const int MinPointsToWin = 4;
    private const int MinPointsDifferenceToWin = 2;

    private const int MinTieBreakPointsToWin = 7;

    private const int MinGamesToWin = 6;
    private const int MinGamesDifferenceToWin = 2;

    public int FirstPlayerId { get; set; } = firstPlayerId;
    public int SecondPlayerId { get; set; } = secondPlayerId;

    public Dictionary<int, MatchState> PlayersMatchStates { get; set; } = new Dictionary<int, MatchState> {
        { firstPlayerId, new MatchState() },
        { secondPlayerId, new MatchState() }
    };

    public void StartNewGame() {
        foreach (var playerMatchState in PlayersMatchStates.Values)
            playerMatchState.Reset();
    }

    public void StartNewSet() {
        foreach (var playerMatchState in PlayersMatchStates.Values)
            playerMatchState.FinishedGames.Add(playerMatchState.GamesInCurrentSet);
    }
    
    private MatchState GetOpponentMatchState(int playerId) {
        return PlayersMatchStates.Where(x => x.Key != playerId).First().Value;
    }

    public bool IsGameFinished() {
        if (IsTieBreak())
            return IsTieBreakFinished();
        else
            return IsRegularGameFinished();
    }

    private bool IsRegularGameFinished() {
        var (possibleWinnerId, possibleWinner) = PlayersMatchStates.FirstOrDefault(
            x => x.Value.PointsInCurrentGame >= MinPointsToWin
        );
        if (possibleWinner == null)
            return false;
        var possibleLoser = GetOpponentMatchState(possibleWinnerId);

        return Math.Abs(possibleWinner.PointsInCurrentGame - possibleLoser.PointsInCurrentGame)
            >= MinPointsDifferenceToWin;
    }

    public bool IsTieBreak() {
        return PlayersMatchStates.All(x => x.Value.GamesInCurrentSet == MinGamesToWin);
    }

    public bool IsTieBreakFinished() {
        var (possibleWinnerId, possibleWinner) = PlayersMatchStates.FirstOrDefault(
            x => x.Value.PointsInCurrentGame >= MinTieBreakPointsToWin
        );
        if (possibleWinner == null)
            return false;
        var possibleLoser = GetOpponentMatchState(possibleWinnerId);

        return Math.Abs(possibleWinner.PointsInCurrentGame - possibleLoser.PointsInCurrentGame)
            >= MinPointsDifferenceToWin;
    }

    public bool IsSetFinished() {
        var (possibleWinnerId, possibleWinner) = PlayersMatchStates.FirstOrDefault(
            x => x.Value.GamesInCurrentSet >= MinGamesToWin
        );
        if (possibleWinner == null)
            return false;
        var possibleLoser = GetOpponentMatchState(possibleWinnerId);

        if (IsTieBreak())
            return IsTieBreakFinished();

        return Math.Abs(possibleWinner.GamesInCurrentSet - possibleLoser.GamesInCurrentSet)
            >= MinGamesDifferenceToWin;
    }

    public bool IsMatchFinished() {
        return PlayersMatchStates.Any(kv => {
            var opponent = GetOpponentMatchState(kv.Key);
            int sum = 0;
            for (var i = 0; i < kv.Value.FinishedGames.Count; i++)
                if (kv.Value.FinishedGames[i] > opponent.FinishedGames[i])
                    sum++;
            return sum == 2;
        });
    }
}