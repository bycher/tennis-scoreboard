using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Models.ViewModels;

namespace TennisScoreboard.Controllers;

[Route("/")]
public class HomeController : Controller {
    public IActionResult Index() =>  View();

    [Route("/error")]
    public IActionResult Error() {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        return View(new ErrorViewModel {
            Message = "An unexpected error occured.",
            Details = exceptionFeature?.Error.Message
        });
    }
}