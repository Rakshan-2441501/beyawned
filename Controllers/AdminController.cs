using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beyawned.Models;

namespace Beyawned.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var submissions = await _context.ContactSubmissions
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
            
        return View(submissions);
    }
}
