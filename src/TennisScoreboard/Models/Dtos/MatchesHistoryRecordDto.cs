namespace TennisScoreboard.Models.Dtos;

public class MatchesHistoryRecordDto {
    public string FirstPlayerName { get; set; } = null!;
    public string SecondPlayerName { get; set; } = null!;
    public string WinnerName { get; set; } = null!;

    public bool IsFitUnderFilter(MatchesHistoryFilterDto filter) => filter.IsAppliedToPlayer(FirstPlayerName) ||
                                                                    filter.IsAppliedToPlayer(SecondPlayerName);
}