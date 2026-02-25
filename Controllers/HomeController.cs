using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Beyawned.Models;

namespace Beyawned.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

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

        var submission = new ContactSubmission
        {
            Name = model.Name,
            Email = model.Email,
            Industry = model.Industry ?? string.Empty,
            Message = model.Message ?? string.Empty
        };

        _context.ContactSubmissions.Add(submission);
        _context.SaveChanges();

        TempData["ContactSuccess"] = "Thanks! We've received your submission and will get back to you within a few hours.";
        return RedirectToAction(nameof(Index), new { anchor = "contact" });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
