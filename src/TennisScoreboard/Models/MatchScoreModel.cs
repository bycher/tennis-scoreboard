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

    public bool IsAdvantage => (FirstPlayerScores.Points == MinPointsToWin ||
                            SecondPlayerScores.Points == MinPointsToWin) &&
                            PointsDifference == MinPointsDifferenceToWin - 1;

    public bool IsGameFinished => IsTieBreak ? IsTieBreakFinished : IsRegularGameFinished;

    public bool IsTieBreak => FirstPlayerScores.Games >= MinGamesToWin &&
                            SecondPlayerScores.Games >= MinGamesToWin;

    public bool IsSetFinished => IsTieBreak ? IsTieBreakFinished
                                            : (FirstPlayerScores.Games >= MinGamesToWin ||
                                            SecondPlayerScores.Games >= MinGamesToWin) &&
                                            GamesDifference >= MinGamesDifferenceToWin;

    public bool IsMatchFinished {
        get {
            var firstWinSetsCount = Enumerable.Zip(FirstPlayerScores.Sets, SecondPlayerScores.Sets)
                                            .Count(x => x.First > x.Second);
            var secondWinSetsCount = SecondPlayerScores.Sets.Count - firstWinSetsCount;
            return firstWinSetsCount == MinSetsToWin || secondWinSetsCount == MinSetsToWin;
        }
    }

    private bool IsTieBreakFinished => (FirstPlayerScores.Points >= MinTieBreakPointsToWin ||
                                    SecondPlayerScores.Points >= MinTieBreakPointsToWin) &&
                                    PointsDifference >= MinPointsDifferenceToWin;
    
    private bool IsRegularGameFinished => (FirstPlayerScores.Points >= MinPointsToWin ||
                                        SecondPlayerScores.Points >= MinPointsToWin) &&
                                        PointsDifference >= MinPointsDifferenceToWin;

    private int PointsDifference => Math.Abs(FirstPlayerScores.Points - SecondPlayerScores.Points);
    private int GamesDifference => Math.Abs(FirstPlayerScores.Games - SecondPlayerScores.Games);

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