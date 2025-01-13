namespace TennisScoreboard.Models;

public class MatchHistoryViewModel
{
    public IEnumerable<MatchHistoryRecord> Records { get; set; } = [];
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string? FilterByPlayerName { get; set; }

    public int NextPage => CurrentPage + 1;
    public int PreviousPage => CurrentPage - 1;

    public string GetCssForPage(int page)
    {
        return page == CurrentPage ? "active" : "";
    }
}