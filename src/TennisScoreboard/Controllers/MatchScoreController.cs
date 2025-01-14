using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("match-score")]
public class MatchScoreController(
    OngoingMatchesStorage ongoingMatchesStorage,
    MatchScoreCalculationService matchScoreCalculationService,
    FinishedMatchesArchiveService finishedMatchesArchiveService) : Controller
{
    private readonly OngoingMatchesStorage _ongoingMatchesStorage = ongoingMatchesStorage;
    private readonly MatchScoreCalculationService _matchScoreCalculationService = matchScoreCalculationService;
    private readonly FinishedMatchesArchiveService _finishedMatchesArchiveService = finishedMatchesArchiveService;

    public IActionResult Index(Guid uuid)
    {
        var match = _ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match witch guid {uuid} was not found"});

        return View(new MatchScoreViewModel(match, uuid));
    }

    [HttpPost]
    public async Task<IActionResult> Index(Guid uuid, int winnerId)
    {
        var match = _ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match '{uuid}' was not found"});
        
        _matchScoreCalculationService.UpdateMatchScore(match, winnerId);
        if (match.IsMatchFinished)
        {
            await _finishedMatchesArchiveService.ArchiveMatch(match.Match, winnerId);
            if (!_ongoingMatchesStorage.Remove(uuid))
                return StatusCode(500, new { message = $"Failed to remove match '{uuid}' from storage" });
        }

        return View(new MatchScoreViewModel(match, uuid));
    }
}