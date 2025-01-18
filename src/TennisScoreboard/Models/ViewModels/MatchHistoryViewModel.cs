using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Models.ViewModels;

public class MatchHistoryViewModel(
    IEnumerable<MatchesHistoryRecordDto> records, int currentPage, string? filterByPlayerName
) {
    private const int PageSize = 5;

    public IEnumerable<MatchesHistoryRecordDto> Records { get; set; } = records;
    public int CurrentPage { get; set; } = currentPage;
    public string? FilterByPlayerName { get; set; } = filterByPlayerName;

    public int NextPage => CurrentPage + 1;
    public int PreviousPage => CurrentPage - 1;

    public IEnumerable<MatchesHistoryRecordDto> PagedRecords =>
        Records.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

    public int TotalPages => (int)Math.Ceiling(Records.Count() / (double)PageSize);
}