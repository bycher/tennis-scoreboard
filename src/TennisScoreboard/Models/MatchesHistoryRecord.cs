namespace TennisScoreboard.Models;

public class MatchesHistoryRecord
{
    public string FirstPlayerName { get; set; } = null!;
    public string SecondPlayerName { get; set; } = null!;
    public string WinnerName { get; set; } = null!;

    public bool IsFitUnderFilter(MatchesHistoryFilter filter) =>
        filter.IsAppliedToPlayer(FirstPlayerName) || filter.IsAppliedToPlayer(SecondPlayerName);
}