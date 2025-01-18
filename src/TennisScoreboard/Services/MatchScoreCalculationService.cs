using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService {
    public void UpdateMatchScore(MatchScoreUpdateContextDto context) {
        context.MatchScore.AddPoint(context.WinnerId);
        
        if (context.MatchScore.IsGameFinished) {
            context.MatchScore.AddGame(context.WinnerId);

            if (!context.MatchScore.IsMatchFinished) {
                if (context.MatchScore.IsSetFinished)
                    context.MatchScore.StartNewSet();
                    
                context.MatchScore.StartNewGame();
            }
        }
    }
}