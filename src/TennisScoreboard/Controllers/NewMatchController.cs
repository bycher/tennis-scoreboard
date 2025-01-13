using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Controllers;

[Route("new-match")]
public class NewMatchController : Controller
{
    private readonly TennisMatchesContext _context;
    private readonly OngoingMatchesStorage _ongoingMatchesStorage;

    public NewMatchController(TennisMatchesContext context, OngoingMatchesStorage ongoingMatchesStorage)
    {
        _context = context;
        _ongoingMatchesStorage = ongoingMatchesStorage;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(string firstPlayerName, string secondPlayerName)
    {
        await AddPlayer(firstPlayerName);
        await AddPlayer(secondPlayerName);

        var firstPlayer = _context.Players.Single(p => p.Name == firstPlayerName);
        var secondPlayer = _context.Players.Single(p => p.Name == secondPlayerName);
        
        var match = new Match
        {
            FirstPlayerId = firstPlayer.Id,
            SecondPlayerId = secondPlayer.Id,
            FirstPlayer = firstPlayer,
            SecondPlayer = secondPlayer
        };
        var key = _ongoingMatchesStorage.Add(new MatchScore(match));

        return Redirect($"/match-score?uuid={key}");
    }

    private async Task AddPlayer(string name)
    {
        try
        {
            var player = new Player { Name = name };
            if (_context.Players.Any(p => p.Name == name))
                return;
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Player already exists. Do nothing.");
        }
    }
}