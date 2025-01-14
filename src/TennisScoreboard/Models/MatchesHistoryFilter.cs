namespace TennisScoreboard.Models;

public class MatchesHistoryFilter(string? playerName)
{
    public string? PlayerName { get; set; } = playerName;

    public bool IsAppliedToPlayer(string playerName) =>
        string.IsNullOrWhiteSpace(PlayerName) || playerName.Contains(PlayerName);
}