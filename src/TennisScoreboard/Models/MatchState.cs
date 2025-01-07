namespace TennisScoreboard.Models;

public class MatchState {
    public int PointsInCurrentGame { get; set; }
    public int GamesInCurrentSet { get; set; }
    public List<int> FinishedGames { get; set; } = [];

    public void Reset() {
        PointsInCurrentGame = 0;
    }
}