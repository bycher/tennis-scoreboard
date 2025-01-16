using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Models.ViewModels;

public class MatchHistoryViewModel
{
    private const int PageSize = 5;

    public IEnumerable<MatchesHistoryRecordDto> Records { get; set; } = [];
    public int CurrentPage { get; set; }
    public string? FilterByPlayerName { get; set; }

    public int NextPage => CurrentPage + 1;
    public int PreviousPage => CurrentPage - 1;

    public IEnumerable<MatchesHistoryRecordDto> PagedRecords => Records.Skip((CurrentPage - 1) * PageSize)
                                                                       .Take(PageSize);

    public int TotalPages => (int)Math.Ceiling(Records.Count() / (double)PageSize);
}