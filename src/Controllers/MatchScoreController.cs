using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard.Controllers;

public class MatchScoreController : Controller {
    public IActionResult Index() {
        return View();
    }
    
    [Route("match-score/{uuid}")]
    public IActionResult MatchScore(string uuid) {
        return View();
    }
}