using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Requests;
using TennisScoreboard.Models.ViewModels;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("matches")]
public class MatchesController(MatchesHistoryService matchesHistoryService) : Controller
{
    private readonly MatchesHistoryService _matchesHistoryService = matchesHistoryService;

    public async Task<IActionResult> GetMatches(GetMatchesRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var filter = new MatchesHistoryFilterDto(request.FilterByPlayerName);
        var records = await _matchesHistoryService.GetFilteredHistory(filter);

        return View(new MatchHistoryViewModel
        {
            Records = records,
            CurrentPage = request.Page,
            FilterByPlayerName = request.FilterByPlayerName
        });
    }
}