using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService {
    public void UpdateMatchScore(MatchScoreModel matchScoreModel, int winnerId) {
        if (winnerId == matchScoreModel.Match.FirstPlayerId)
            matchScoreModel.FirstPlayerScores.PointsInCurrentGame++;
        else
            matchScoreModel.SecondPlayerScores.PointsInCurrentGame++;
        
        if (matchScoreModel.IsGameFinished) {
            if (winnerId == matchScoreModel.Match.FirstPlayerId)
                matchScoreModel.FirstPlayerScores.GamesInCurrentSet++;
            else
                matchScoreModel.SecondPlayerScores.GamesInCurrentSet++;
            if (matchScoreModel.IsSetFinished && !matchScoreModel.IsMatchFinished)
                matchScoreModel.StartNewSet();
            matchScoreModel.StartNewGame();
        }
    }
}