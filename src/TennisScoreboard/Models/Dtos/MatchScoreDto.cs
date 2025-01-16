using TennisScoreboard.Models.Entities;

namespace TennisScoreboard.Models.Dtos;

public class MatchScoreDto
{
    public Player FirstPlayer { get; set; }
    public Player SecondPlayer { get; set; }

    private readonly Dictionary<int, PlayerScoresDto> _playersScores = [];

    public MatchScoreDto(Player firstPlayer, Player secondPlayer)
    {
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;

        _playersScores[firstPlayer.Id] = new PlayerScoresDto();
        _playersScores[secondPlayer.Id] = new PlayerScoresDto();
    }

    private const int MinPointsToWin = 4;
    private const int MinTieBreakPointsToWin = 7;
    private const int MinPointsDifferenceToWin = 2;

    private const int MinGamesToWin = 6;
    private const int MinGamesDifferenceToWin = 2;

    private const int MinSetsToWin = 2;

    public int NumberOfSets => _playersScores[FirstPlayer.Id].Sets.Count;

    public bool IsGameFinished => IsTieBreak ? IsTieBreakFinished : IsRegularGameFinished;

    public bool IsSetFinished => IsTieBreak ? IsTieBreakFinished : IsRegularSetFinished;

    public bool IsMatchFinished
    {
        get
        {
            var setPairs = new List<(int First, int Second)>();

            for (int i = 0; i < NumberOfSets; i++)
            {
                var setPair = _playersScores.Values.Select(ps => ps.Sets[i]).ToArray();
                if (setPair is [var first, var second])
                    setPairs.Add((first, second));
            }

            return setPairs.Count(sp => sp.First > sp.Second) >= MinSetsToWin
                || setPairs.Count(sp => sp.First < sp.Second) >= MinSetsToWin; 
        }
    }

    public bool IsTieBreak => _playersScores.Values.All(ps => ps.Games >= MinGamesToWin);

    public bool IsAdvantage => !IsTieBreak
                            && _playersScores.Values.Any(ps => ps.Points == MinPointsToWin)
                            && PointsDifference == MinPointsDifferenceToWin - 1;

    private bool IsTieBreakFinished => IfAnyPlayerPassedPointsThreshold(MinTieBreakPointsToWin)
                                    && PointsDifference >= MinPointsDifferenceToWin;

    private bool IsRegularGameFinished => IfAnyPlayerPassedPointsThreshold(MinPointsToWin)
                                       && PointsDifference >= MinPointsDifferenceToWin;

    private bool IsRegularSetFinished => IfAnyPlayerPassedGamesThreshold(MinGamesToWin)
                                      && GamesDifference >= MinGamesDifferenceToWin;

    private bool IfAnyPlayerPassedPointsThreshold(int threshold) =>
        _playersScores.Values.Any(ps => ps.Points >= threshold);
    
    private bool IfAnyPlayerPassedGamesThreshold(int threshold) =>
        _playersScores.Values.Any(ps => ps.Games >= threshold);

    private int PointsDifference
    {
        get
        {
            var firstPlayerPoints = _playersScores[FirstPlayer.Id].Points;
            var secondPlayerPoints = _playersScores[SecondPlayer.Id].Points;

            return Math.Abs(firstPlayerPoints - secondPlayerPoints);
        }
    }

    private int GamesDifference
    {
        get
        {
            var firstPlayerGames = _playersScores[FirstPlayer.Id].Games;
            var secondPlayerGames = _playersScores[SecondPlayer.Id].Games;
            
            return Math.Abs(firstPlayerGames - secondPlayerGames);
        }
    }

    public void StartNewGame()
    {
        foreach (var playerScores in _playersScores.Values)
            playerScores.ResetPoints();
    }

    public void StartNewSet()
    {
        foreach (var playerScores in _playersScores.Values)
            playerScores.ResetGames();
    }

    public void AddPoint(int winnerId)
    {
        var loserId = winnerId == FirstPlayer.Id ? SecondPlayer.Id : FirstPlayer.Id;

        if (IsAdvantage)
            _playersScores[loserId].Points--;
        else
            _playersScores[winnerId].Points++;
    }

    public void AddGame(int winnerId) => _playersScores[winnerId].Games++;

    public bool TryGetScoreComponent<T>(
        int playerId, string componentName, out T? component, object?[]? parameters = null)
    {
        component = default;
        if (!_playersScores.TryGetValue(playerId, out var playerScores))
            return false;

        var property = playerScores.GetType().GetProperty(componentName);
        if (property != null)
        {
            component = (T?)property.GetValue(playerScores);
            return true;
        }

        var method = playerScores.GetType().GetMethod(componentName);
        component = (T?)method?.Invoke(playerScores, parameters);

        return component != null;
    }

    public void SetScoreComponents<T>(T firstPlayerComponent, T secondPlayerComponent, string componentName)
    {
        var property = _playersScores[FirstPlayer.Id].GetType().GetProperty(componentName);

        property?.SetValue(_playersScores[FirstPlayer.Id], firstPlayerComponent);
        property?.SetValue(_playersScores[SecondPlayer.Id], secondPlayerComponent);
    }
}