namespace TennisScoreboard.Models.Dtos;

public class MatchesHistoryFilterDto(string? playerName) {
    public string? PlayerName { get; set; } = playerName;

    public bool IsAppliedToPlayer(string playerName) => string.IsNullOrWhiteSpace(PlayerName) ||
                                                        playerName.Contains(PlayerName);
}