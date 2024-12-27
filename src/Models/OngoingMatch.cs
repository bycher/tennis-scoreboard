namespace TennisScoreboard.Models;

public class OngoingMatch(int firstPlayerId, int secondPlayerId)
{
    public int FirstPlayerId { get; set; } = firstPlayerId;
    public int SecondPlayerId { get; set; } = secondPlayerId;
    public int FirstPlayerPoints { get; set; }
    public int SecondPlayerPoints { get; set; }
    public List<int> FirstPlayerGames { get; set; } = [];
    public List<int> SecondPlayerGames { get; set; } = [];
}