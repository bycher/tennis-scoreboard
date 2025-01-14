using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("matches")]
public class MatchesController(FinishedMatchesArchiveService finishedMatchesArchiveService) : Controller
{
    private readonly FinishedMatchesArchiveService _finishedMatchesArchiveService = finishedMatchesArchiveService;

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