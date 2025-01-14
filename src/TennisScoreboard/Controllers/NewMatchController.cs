using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

[Route("new-match")]
public class NewMatchController(
    OngoingMatchesStorage ongoingMatchesStorage, PlayersService playersService) : Controller
{
    private readonly PlayersService _playersService = playersService;
    private readonly OngoingMatchesStorage _ongoingMatchesStorage = ongoingMatchesStorage;

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(NewMatch newMatch)
    {
        if (!ModelState.IsValid)
            return View(newMatch);

        var firstPlayer = await _playersService.AddPlayer(newMatch.FirstPlayerName!);
        var secondPlayer = await _playersService.AddPlayer(newMatch.SecondPlayerName!);

        var key = _ongoingMatchesStorage.Add(firstPlayer, secondPlayer);

        return Redirect($"/match-score?uuid={key}");
    }
}