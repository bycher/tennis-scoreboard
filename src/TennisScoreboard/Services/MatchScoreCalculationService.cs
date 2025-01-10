using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService {
    public void UpdateMatchScore(MatchScoreModel matchScoreModel, int winnerId) {
        UpdatePoints(matchScoreModel, winnerId);
        
        if (matchScoreModel.IsGameFinished) {
            UpdateGames(matchScoreModel, winnerId);

            if (!matchScoreModel.IsMatchFinished) {
                if (matchScoreModel.IsSetFinished)
                    matchScoreModel.StartNewSet();
                matchScoreModel.StartNewGame();
            }
        }
    }

    private static void UpdateGames(MatchScoreModel matchScoreModel, int winnerId) {
        if (winnerId == matchScoreModel.Match.FirstPlayerId)
            matchScoreModel.FirstPlayerScores.Games++;
        else
            matchScoreModel.SecondPlayerScores.Games++;
    }

    private static void UpdatePoints(MatchScoreModel matchScoreModel, int winnerId) {
        if (winnerId == matchScoreModel.Match.FirstPlayerId) {
            if (matchScoreModel.IsAdvantage)
                matchScoreModel.SecondPlayerScores.Points--;
            else
                matchScoreModel.FirstPlayerScores.Points++;
        }
        else {
            if (matchScoreModel.IsAdvantage)
                matchScoreModel.FirstPlayerScores.Points--;
            else
                matchScoreModel.SecondPlayerScores.Points++;
        }
    }
}