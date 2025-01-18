using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Requests;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("new-match")]
public class NewMatchController(
    OngoingMatchesService ongoingMatchesService, PlayersService playersService) : Controller {
    public IActionResult StartNewMatch() => View();

    [HttpPost]
    public async Task<IActionResult> StartNewMatch(NewMatchRequest request) {
        if (!ModelState.IsValid)
            return View(request);

        var (firstPlayer, secondPlayer) = await playersService.AddPlayers(
            request.FirstPlayerName, request.SecondPlayerName
        );
            
        var matchScore = new MatchScoreDto(firstPlayer, secondPlayer);
        var key = ongoingMatchesService.Add(matchScore);

        return RedirectToAction("GetMatchScore", "MatchScore", new { uuid = key });
    }
}