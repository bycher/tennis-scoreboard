namespace TennisScoreboard.Models;

public class MatchScoreModel(Match match) {
    private const int MinPointsToWin = 4;
    private const int MinTieBreakPointsToWin = 7;
    private const int MinPointsDifferenceToWin = 2;
    private const int MinGamesToWin = 6;
    private const int MinGamesDifferenceToWin = 2;
    private const int MinSetsToWin = 2;

    public Match Match { get; set; } = match;
    public Scores FirstPlayerScores { get; set; } = new Scores();
    public Scores SecondPlayerScores { get; set; } = new Scores();

    public bool IsGameFinished => IsTieBreak ? IsTieBreakFinished : IsRegularGameFinished;

    public bool IsTieBreak => FirstPlayerScores.GamesInCurrentSet >= MinGamesToWin &&
                            SecondPlayerScores.GamesInCurrentSet >= MinGamesToWin;

    public bool IsSetFinished => IsTieBreak ? IsTieBreakFinished
                                            : (FirstPlayerScores.GamesInCurrentSet >= MinGamesToWin ||
                                            SecondPlayerScores.GamesInCurrentSet >= MinGamesToWin) &&
                                            GamesDifference >= MinGamesDifferenceToWin;

    public bool IsMatchFinished {
        get {
            var firstWinSetsCount = Enumerable.Zip(FirstPlayerScores.FinishedSets, SecondPlayerScores.FinishedSets)
                                              .Count(x => x.First > x.Second);
            var secondWinSetsCount = SecondPlayerScores.FinishedSets.Count - firstWinSetsCount;
            return firstWinSetsCount == MinSetsToWin || secondWinSetsCount == MinSetsToWin;
        }
    }

    private bool IsTieBreakFinished => (FirstPlayerScores.PointsInCurrentGame >= MinTieBreakPointsToWin ||
                                    SecondPlayerScores.PointsInCurrentGame >= MinTieBreakPointsToWin) &&
                                    PointsDifference >= MinPointsDifferenceToWin;
    
    private bool IsRegularGameFinished => (FirstPlayerScores.PointsInCurrentGame >= MinPointsToWin ||
                                        SecondPlayerScores.PointsInCurrentGame >= MinPointsToWin) &&
                                        PointsDifference >= MinPointsDifferenceToWin;

    private int PointsDifference =>
        Math.Abs(FirstPlayerScores.PointsInCurrentGame - SecondPlayerScores.PointsInCurrentGame);

    private int GamesDifference =>
        Math.Abs(FirstPlayerScores.GamesInCurrentSet - SecondPlayerScores.GamesInCurrentSet);

    public MatchScoreModel(int firstPlayerId, int secondPlayerId) : this(new Match {
        FirstPlayerId = firstPlayerId,
        SecondPlayerId = secondPlayerId,
    }) {
    }

    public void StartNewGame() {
        FirstPlayerScores.ResetPoints();
        SecondPlayerScores.ResetPoints();
    }

    public void StartNewSet() {
        FirstPlayerScores.ResetGames();
        SecondPlayerScores.ResetGames();
    }
}