using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

public class MatchScoreController(
    OngoingMatchesStorage ongoingMatchesStorage,
    MatchScoreCalculationService matchScoreCalculationService, 
    FinishedMatchesArchiveService finishedMatchesArchiveService
) : Controller {
    [HttpGet]
    [Route("match-score")]
    public IActionResult GetMatchScore([FromQuery] Guid uuid) {
        var match = ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match witch guid {uuid} was not found"});

        return View(match);
    }

    [HttpPost]
    [Route("match-score")]
    public IActionResult UpdateMatchScore([FromQuery] Guid uuid, [FromForm] int winnerId) {
        var match = ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match '{uuid}' was not found"});

        if (!match.IsMatchFinished)
            matchScoreCalculationService.UpdateMatchScore(match, winnerId);
        else {
            match.Match.WinnerId = winnerId;
            finishedMatchesArchiveService.ArchiveMatch(match);
            if (!ongoingMatchesStorage.Remove(uuid))
                return StatusCode(500, new { message = $"Failed to remove match '{uuid}' from storage" });
        }

        return View("GetMatchScore", match);
    }
}