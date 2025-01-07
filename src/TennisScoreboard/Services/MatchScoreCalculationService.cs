using TennisScoreboard.Models;

namespace TennisScoreboard.Services;

public class MatchScoreCalculationService {
    public void UpdateMatchScore(OngoingMatch match, int winnerId) {
        var winnerMatchState = match.PlayersMatchStates[winnerId];
        winnerMatchState.PointsInCurrentGame++;
        
        if (match.IsGameFinished()) {
            match.PlayersMatchStates[winnerId].GamesInCurrentSet++;
            if (match.IsSetFinished() && !match.IsMatchFinished())
                match.StartNewSet();
            match.StartNewGame();
        }
    }
}