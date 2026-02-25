using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Beyawned.Models;

namespace Beyawned.Controllers;

public class HomeController : Controller
{
    // GET /
    public IActionResult Index()
    {
        return View(new ContactViewModel());
    }

    // POST / (contact form)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ScrollToContact = true;
            return View(model);
        }

        // TODO: wire up email / CRM
        TempData["ContactSuccess"] = "Thanks! We'll get back to you within a few hours.";
        return RedirectToAction(nameof(Index), new { anchor = "contact" });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
