using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Requests;
using TennisScoreboard.Models.ViewModels;
using TennisScoreboard.Services;
using TennisScoreboard.Utils;

namespace TennisScoreboard.Controllers;

[Route("match-score")]
public class MatchScoreController(
    OngoingMatchesService ongoingMatchesService,
    MatchScoreCalculationService matchScoreCalculationService,
    MatchesHistoryService matchesHistoryService) : Controller {
    private const string MatchScoreViewName = "MatchScore";

    private static string MatchNotFoundMessage(Guid uuid) => string.Format($"Match '{uuid}' was not found");

    public IActionResult GetMatchScore([ValidGuid] Guid uuid) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var matchScore = ongoingMatchesService.Get(uuid);
        if (matchScore is null)
            return NotFound(new { message = MatchNotFoundMessage(uuid) });

        return View(MatchScoreViewName, new MatchScoreViewModel(matchScore, uuid));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMatchScore(UpdateMatchScoreRequest request) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var matchScore = ongoingMatchesService.Get(request.Uuid);
        if (matchScore is null)
            return NotFound(new { message = MatchNotFoundMessage(request.Uuid) });
        
        var context = new MatchScoreUpdateContextDto(matchScore, request.WinnerId);
        if (!context.IsValid)
            return BadRequest(new { message = "Winner ID must be equal to one of player's ID" });

        matchScoreCalculationService.UpdateMatchScore(context);
        if (matchScore.IsMatchFinished)
            await matchesHistoryService.AddToHistory(context);

        return View(MatchScoreViewName, new MatchScoreViewModel(matchScore, request.Uuid, request.WinnerId));
    }
}