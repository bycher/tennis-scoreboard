using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService
{
    public void UpdateMatchScore(MatchScoreUpdateContextDto context)
    {
        UpdatePoints(context);
        
        if (context.MatchScore.IsGameFinished)
        {
            UpdateGames(context);

            if (!context.MatchScore.IsMatchFinished)
            {
                if (context.MatchScore.IsSetFinished)
                    context.MatchScore.StartNewSet();

                context.MatchScore.StartNewGame();
            }
        }
    }

    private static void UpdateGames(MatchScoreUpdateContextDto context)
    {
        if (context.WinnerId == context.MatchScore.Match.FirstPlayerId)
            context.MatchScore.FirstPlayerScores.Games++;
        else
            context.MatchScore.SecondPlayerScores.Games++;
    }

    private static void UpdatePoints(MatchScoreUpdateContextDto context)
    {
        if (context.WinnerId == context.MatchScore.Match.FirstPlayerId)
        {
            if (context.MatchScore.IsAdvantage)
                context.MatchScore.SecondPlayerScores.Points--;
            else
                context.MatchScore.FirstPlayerScores.Points++;
        }
        else
        {
            if (context.MatchScore.IsAdvantage)
                context.MatchScore.FirstPlayerScores.Points--;
            else
                context.MatchScore.SecondPlayerScores.Points++;
        }
    }
}