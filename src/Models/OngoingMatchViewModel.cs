namespace TennisScoreboard.Models;

public class OngoingMatchViewModel {
    public string FirstPlayerName { get; set; } = null!;
    public string SecondPlayerName { get; set; } = null!;

    public int FirstPlayerPoints { get; set; }
    public int SecondPlayerPoints { get; set; }

    public int FirstPlayerGames { get; set; }
    public int SecondPlayerGames { get; set; }

    public List<int> FirstPlayerSets { get; set; } = [];
    public List<int> SecondPlayerSets { get; set; } = [];

    public int FirstPlayerId { get; set; }
    public int SecondPlayerId { get; set; }
}