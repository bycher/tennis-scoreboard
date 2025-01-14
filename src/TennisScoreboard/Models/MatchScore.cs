namespace TennisScoreboard.Models;

public class MatchScore
{
    private const int MinPointsToWin = 4;
    private const int MinTieBreakPointsToWin = 7;
    private const int MinPointsDifferenceToWin = 2;

    private const int MinGamesToWin = 6;
    private const int MinGamesDifferenceToWin = 2;

    private const int MinSetsToWin = 2;

    public Match Match { get; set; }
    public Scores FirstPlayerScores { get; set; } = new();
    public Scores SecondPlayerScores { get; set; } = new();

    public MatchScore(Match match)
    {
        Match = match;
    }

    public MatchScore(int firstPlayerId, int secondPlayerId)
    {
        Match = new Match
        {
            FirstPlayerId = firstPlayerId,
            SecondPlayerId = secondPlayerId,
        };
    }

    public bool IsAdvantage => (FirstPlayerScores.Points == MinPointsToWin
                            || SecondPlayerScores.Points == MinPointsToWin)
                            && PointsDifference == MinPointsDifferenceToWin - 1;

    public bool IsGameFinished => IsTieBreak ? IsTieBreakFinished : IsRegularGameFinished;

    public bool IsTieBreak => FirstPlayerScores.Games >= MinGamesToWin
                           && SecondPlayerScores.Games >= MinGamesToWin;

    public bool IsSetFinished => IsTieBreak ? IsTieBreakFinished : IsRegularSetFinished;

    public bool IsMatchFinished
    {
        get
        {
            var firstWinSetsCount = Enumerable.Zip(FirstPlayerScores.Sets, SecondPlayerScores.Sets)
                                              .Count(x => x.First > x.Second);
            var secondWinSetsCount = SecondPlayerScores.Sets.Count - firstWinSetsCount;
            return firstWinSetsCount == MinSetsToWin || secondWinSetsCount == MinSetsToWin;
        }
    }

    private bool IsRegularSetFinished => (FirstPlayerScores.Games >= MinGamesToWin
                                      || SecondPlayerScores.Games >= MinGamesToWin)
                                      && GamesDifference >= MinGamesDifferenceToWin;

    private bool IsTieBreakFinished => (FirstPlayerScores.Points >= MinTieBreakPointsToWin
                                    || SecondPlayerScores.Points >= MinTieBreakPointsToWin)
                                    && PointsDifference >= MinPointsDifferenceToWin;
    
    private bool IsRegularGameFinished => (FirstPlayerScores.Points >= MinPointsToWin
                                       || SecondPlayerScores.Points >= MinPointsToWin)
                                       && PointsDifference >= MinPointsDifferenceToWin;

    private int PointsDifference => Math.Abs(FirstPlayerScores.Points - SecondPlayerScores.Points);
    private int GamesDifference => Math.Abs(FirstPlayerScores.Games - SecondPlayerScores.Games);

    public void StartNewGame()
    {
        FirstPlayerScores.ResetPoints();
        SecondPlayerScores.ResetPoints();
    }

    public void StartNewSet()
    {
        FirstPlayerScores.ResetGames();
        SecondPlayerScores.ResetGames();
    }

    public string WinnerName => Match.WinnerId == Match.FirstPlayerId
                                ? Match.FirstPlayer.Name
                                : Match.SecondPlayer.Name;
}