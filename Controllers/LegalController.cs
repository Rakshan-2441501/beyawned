using Microsoft.AspNetCore.Mvc;

namespace Beyawned.Controllers;

public class LegalController : Controller
{
    public IActionResult Terms()    => View();
    public IActionResult Privacy()  => View();
    public IActionResult Refund()   => View();
    public IActionResult Delivery() => View();
}
