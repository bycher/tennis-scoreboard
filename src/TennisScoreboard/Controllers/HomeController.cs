using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard.Controllers;

[Route("/")]
public class HomeController : Controller
{
    public IActionResult Index() =>  View();
}   