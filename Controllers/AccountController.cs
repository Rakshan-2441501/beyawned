using Microsoft.AspNetCore.Mvc;
using Beyawned.Models;
using System.ComponentModel.DataAnnotations;

namespace Beyawned.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        // TODO: implement authentication
        TempData["Error"] = "Invalid credentials. Please try again.";
        return View(model);
    }
}
