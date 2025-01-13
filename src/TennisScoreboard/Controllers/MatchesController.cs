using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("matches")]
public class MatchesController : Controller
{
    private const int PageSize = 5;

    private readonly TennisMatchesContext _context;
    private readonly FinishedMatchesArchiveService _finishedMatchesArchiveService;

    public MatchesController(
        TennisMatchesContext context, FinishedMatchesArchiveService finishedMatchesArchiveService)
    {
        _context = context;
        _finishedMatchesArchiveService = finishedMatchesArchiveService;
    }

    public async Task<IActionResult> Index(
        int page = 1, 
        [FromQuery(Name="filter_by_player_name")] string? filterByPlayerName = null)
    {
        var filteredMatchHistoryRecords = await _finishedMatchesArchiveService.GetFilteredMatchHistoryRecords(
            filterByPlayerName);

        return View(new MatchHistoryViewModel
        {
            Records = filteredMatchHistoryRecords,
            CurrentPage = page,
            FilterByPlayerName = filterByPlayerName
        });
    }
}