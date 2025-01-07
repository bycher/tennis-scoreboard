namespace TennisScoreboard.Models;

public class Match {
    public int Id { get; set; }
    public int FirstPlayerId { get; set; }
    public int SecondPlayerId { get; set; }
    public int WinnerId { get; set; }

    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
}