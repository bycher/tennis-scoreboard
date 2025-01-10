using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;

namespace TennisScoreboard.Controllers;

public class MatchesController(TennisMatchesContext context) : Controller {
    private const int PageSize = 5;

    [HttpGet]
    [Route("matches")]
    public IActionResult GetMatches(
        int page = 1, 
        [FromQuery(Name = "filter_by_player_name")] string? filterByPlayerName = null
    ) {
        var matches = context.Matches
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .AsEnumerable();
        var filtered = matches.Where(m => IsFitUnderFilterName(m, filterByPlayerName));
        var paged = filtered.Skip((page - 1) * PageSize).Take(PageSize);
        
        var matchHistoryPaged = paged.Select(m => new MatchHistoryRecord {
            FirstPlayerName = m.FirstPlayer.Name,
            SecondPlayerName = m.SecondPlayer.Name,
            WinnerName = context.Players.Single(p => p.Id == m.WinnerId).Name
        });

        ViewData["TotalPages"] = (int)Math.Ceiling(filtered.Count() / (double)PageSize);
        ViewData["CurrentPage"] = page;
        ViewData["Filter"] = filterByPlayerName;

        return View(matchHistoryPaged);
    }

    private static bool IsFitUnderFilterName(Match m, string? filterByPlayerName) =>
        string.IsNullOrWhiteSpace(filterByPlayerName) ||
        m.FirstPlayer.Name.Contains(filterByPlayerName) ||
        m.SecondPlayer.Name.Contains(filterByPlayerName);
}