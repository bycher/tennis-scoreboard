using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Controllers;

[Route("matches")]
public class MatchesController : Controller
{
    private const int PageSize = 5;
    private readonly TennisMatchesContext _context;

    public MatchesController(TennisMatchesContext context)
    {
        _context = context;
    }

    public IActionResult Index(
        int page = 1, 
        [FromQuery(Name = "filter_by_player_name")] string? filterByPlayerName = null)
    {
        var matches = _context.Matches
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .AsEnumerable();
        var filtered = matches.Where(m => IsFitUnderFilterName(m, filterByPlayerName));

        var matchHistoryRecords = filtered
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(m => new MatchHistoryRecord
            {
                FirstPlayerName = m.FirstPlayer.Name,
                SecondPlayerName = m.SecondPlayer.Name,
                WinnerName = _context.Players.Single(p => p.Id == m.WinnerId).Name
            });

        return View(new MatchHistoryViewModel
        {
            Records = matchHistoryRecords,
            TotalPages = (int)Math.Ceiling(filtered.Count() / (double)PageSize),
            CurrentPage = page,
            FilterByPlayerName = filterByPlayerName
        });
    }

    private static bool IsFitUnderFilterName(Match match, string? filterByPlayerName)
    {
        return string.IsNullOrWhiteSpace(filterByPlayerName)
            || match.FirstPlayer.Name.Contains(filterByPlayerName)
            || match.SecondPlayer.Name.Contains(filterByPlayerName);
    }
}