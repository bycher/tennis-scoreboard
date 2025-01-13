using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService
{
    public void UpdateMatchScore(MatchScore MatchScore, int winnerId)
    {
        UpdatePoints(MatchScore, winnerId);
        
        if (MatchScore.IsGameFinished)
        {
            UpdateGames(MatchScore, winnerId);

            if (!MatchScore.IsMatchFinished)
            {
                if (MatchScore.IsSetFinished)
                    MatchScore.StartNewSet();
                MatchScore.StartNewGame();
            }
        }
    }

    private static void UpdateGames(MatchScore MatchScore, int winnerId)
    {
        if (winnerId == MatchScore.Match.FirstPlayerId)
            MatchScore.FirstPlayerScores.Games++;
        else
            MatchScore.SecondPlayerScores.Games++;
    }

    private static void UpdatePoints(MatchScore MatchScore, int winnerId)
    {
        if (winnerId == MatchScore.Match.FirstPlayerId)
        {
            if (MatchScore.IsAdvantage)
                MatchScore.SecondPlayerScores.Points--;
            else
                MatchScore.FirstPlayerScores.Points++;
        }
        else
        {
            if (MatchScore.IsAdvantage)
                MatchScore.FirstPlayerScores.Points--;
            else
                MatchScore.SecondPlayerScores.Points++;
        }
    }
}