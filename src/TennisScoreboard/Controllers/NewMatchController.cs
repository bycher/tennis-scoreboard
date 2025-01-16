using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Requests;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("new-match")]
public class NewMatchController(
    OngoingMatchesService ongoingMatchesService, PlayersService playersService) : Controller
{
    private readonly PlayersService _playersService = playersService;
    private readonly OngoingMatchesService _ongoingMatchesService = ongoingMatchesService;

    public IActionResult StartNewMatch() => View();

    [HttpPost]
    public async Task<IActionResult> StartNewMatch(NewMatchRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var (firstPlayer, secondPlayer) = await _playersService.AddPlayers(
            request.FirstPlayerName, request.SecondPlayerName);
            
        var matchScore = new MatchScoreDto(firstPlayer, secondPlayer);
        var key = _ongoingMatchesService.Add(matchScore);

        return RedirectToAction("GetMatchScore", "MatchScore", new { uuid = key });
    }
}