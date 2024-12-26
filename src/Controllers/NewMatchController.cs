using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard.Controllers;

public class NewMatchController : Controller {
    public IActionResult Index() {
        return View();
    }
}