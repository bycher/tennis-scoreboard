using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Controllers;

public class NewMatchController(
    TennisMatchesContext context, OngoingMatchesStorage ongoingMatchesStorage) : Controller {
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    [Route("new-match")]
    public async Task<IActionResult> NewMatch(
        [FromForm] string firstPlayerName, [FromForm] string secondPlayerName
    ) {
        await AddPlayer(firstPlayerName);
        await AddPlayer(secondPlayerName);

        var firstPlayer = context.Players.Single(p => p.Name == firstPlayerName);
        var secondPlayer = context.Players.Single(p => p.Name == secondPlayerName);

        var key = ongoingMatchesStorage.Add(
            new MatchScoreModel(new Match {
                FirstPlayerId = firstPlayer.Id,
                SecondPlayerId = secondPlayer.Id,
                FirstPlayer = firstPlayer,
                SecondPlayer = secondPlayer
            }
        ));

        return RedirectToRoute("match-score", new { uuid = key });
    }

    private async Task AddPlayer(string name) {
        try {
            var player = new Player { Name = name };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException) {
            Console.WriteLine("Player already exists. Do nothing.");
        }
    }
}