using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Controllers;

public class MatchScoreController(
    TennisMatchesContext context,
    OngoingMatchesStorage ongoingMatchesStorage,
    MatchScoreCalculationService matchScoreCalculationService
) : Controller {
    [HttpGet]
    [Route("match-score")]
    public IActionResult GetMatchScore([FromQuery] Guid uuid) {
        var match = ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match witch guid {uuid} was not found"});

        var matchViewModel = new OngoingMatchViewModel {
            FirstPlayerName = context.Players.Single(p => p.Id == match.FirstPlayerId).Name,
            SecondPlayerName = context.Players.Single(p => p.Id == match.SecondPlayerId).Name,
            FirstPlayerPoints = match.PlayersMatchStates[match.FirstPlayerId].PointsInCurrentGame,
            SecondPlayerPoints = match.PlayersMatchStates[match.SecondPlayerId].PointsInCurrentGame,
            FirstPlayerGames = match.PlayersMatchStates[match.FirstPlayerId].GamesInCurrentSet,
            SecondPlayerGames = match.PlayersMatchStates[match.SecondPlayerId].GamesInCurrentSet,
            FirstPlayerSets = match.PlayersMatchStates[match.FirstPlayerId].FinishedGames,
            SecondPlayerSets = match.PlayersMatchStates[match.SecondPlayerId].FinishedGames,
            FirstPlayerId = match.FirstPlayerId,
            SecondPlayerId = match.SecondPlayerId
        };

        return View(matchViewModel);
    }

    [HttpPost]
    [Route("match-score")]
    public IActionResult UpdateMatchScore([FromQuery] Guid uuid, [FromForm] int winnerId) {
        var match = ongoingMatchesStorage.Get(uuid);
        if (match == null)
            return NotFound(new { message = $"Match witch guid {uuid} was not found"});

        matchScoreCalculationService.UpdateMatchScore(match, winnerId);

        var matchViewModel = new OngoingMatchViewModel {
            FirstPlayerName = context.Players.Single(p => p.Id == match.FirstPlayerId).Name,
            SecondPlayerName = context.Players.Single(p => p.Id == match.SecondPlayerId).Name,
            FirstPlayerPoints = match.PlayersMatchStates[match.FirstPlayerId].PointsInCurrentGame,
            SecondPlayerPoints = match.PlayersMatchStates[match.SecondPlayerId].PointsInCurrentGame,
            FirstPlayerGames = match.PlayersMatchStates[match.FirstPlayerId].GamesInCurrentSet,
            SecondPlayerGames = match.PlayersMatchStates[match.SecondPlayerId].GamesInCurrentSet,
            FirstPlayerSets = match.PlayersMatchStates[match.FirstPlayerId].FinishedGames,
            SecondPlayerSets = match.PlayersMatchStates[match.SecondPlayerId].FinishedGames,
            FirstPlayerId = match.FirstPlayerId,
            SecondPlayerId = match.SecondPlayerId
        };

        return View("GetMatchScore", matchViewModel);
    }
}