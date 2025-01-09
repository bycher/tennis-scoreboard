namespace TennisScoreboard.Models;

public class Scores {
    public int PointsInCurrentGame { get; set; }
    public int GamesInCurrentSet { get; set; }
    public List<int> FinishedSets { get; set; } = [];

    public void ResetPoints() {
        PointsInCurrentGame = 0;
    }

    public void ResetGames() {
        FinishedSets.Add(GamesInCurrentSet);
        GamesInCurrentSet = 0;
    }
}