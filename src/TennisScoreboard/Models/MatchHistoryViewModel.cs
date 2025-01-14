namespace TennisScoreboard.Models;

public class MatchHistoryViewModel
{
    private const int PageSize = 5;

    public IEnumerable<MatchesHistoryRecord> Records { get; set; } = [];
    public int CurrentPage { get; set; }
    public string? FilterByPlayerName { get; set; }

    public int NextPage => CurrentPage + 1;
    public int PreviousPage => CurrentPage - 1;

    public string GetCssForPage(int page) => page == CurrentPage ? "active" : "";

    public IEnumerable<MatchesHistoryRecord> PagedRecords => Records
        .Skip((CurrentPage - 1) * PageSize)
        .Take(PageSize);

    public int TotalPages => (int)Math.Ceiling(Records.Count() / (double)PageSize);
}