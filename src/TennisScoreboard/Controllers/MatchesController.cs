using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Controllers;

public class MatchesController(TennisMatchesContext context) : Controller {
    private const int PageSize = 5;

    [HttpGet]
    [Route("matches")]
    public async Task<IActionResult> GetMatches(
        [FromQuery] int page = 1, [FromQuery] string? filterByPlayerName = null
    ) {
        var matches = await context.Matches
            .Where(m => filterByPlayerName == null ||
                        m.FirstPlayer.Name == filterByPlayerName ||
                        m.SecondPlayer.Name == filterByPlayerName)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .ToListAsync();
        

        return View(matches.Select(m => new MatchHistoryRecord {
            FirstPlayerName = m.FirstPlayer.Name,
            SecondPlayerName = m.SecondPlayer.Name,
            WinnerName = context.Players.Single(p => p.Id == m.WinnerId).Name
        }));
    }
}