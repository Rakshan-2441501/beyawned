using Microsoft.AspNetCore.Mvc;

namespace Beyawned.Controllers;

public class ProductsController : Controller
{
    public IActionResult Index() => View();
}
