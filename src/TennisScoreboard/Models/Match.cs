namespace TennisScoreboard.Models;

public class Match
{
    public int Id { get; set; }
    public int FirstPlayerId { get; set; }
    public int SecondPlayerId { get; set; }
    public int WinnerId { get; set; }

    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        if (obj is Match match)
            return match.Id == Id
                && match.FirstPlayerId == FirstPlayerId
                && match.SecondPlayerId == SecondPlayerId
                && match.WinnerId == WinnerId;

        return base.Equals(obj);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, FirstPlayerId, SecondPlayerId, WinnerId);
    }
}