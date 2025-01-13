namespace TennisScoreboard.Models;

public class MatchHistoryRecord
{
    public string FirstPlayerName { get; set; } = null!;
    public string SecondPlayerName { get; set; } = null!;
    public string WinnerName { get; set; } = null!;

    public bool IsFitUnderFilter(string? filterByPlayerName) => string.IsNullOrWhiteSpace(filterByPlayerName)
                                                            || FirstPlayerName.Contains(filterByPlayerName)
                                                            || SecondPlayerName.Contains(filterByPlayerName);
}