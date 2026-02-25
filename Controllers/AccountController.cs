using Microsoft.AspNetCore.Mvc;
using Beyawned.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Beyawned.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Simple SHA256 hashing for demo purposes to match initial seed
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
        var hash = BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == hash);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Admin");
        }

        TempData["Error"] = "Invalid credentials. Please try again.";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        if (remoteError != null)
        {
            TempData["Error"] = $"Error from external provider: {remoteError}";
            return RedirectToAction(nameof(Login));
        }

        var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (info.Principal == null || !info.Principal.Identities.Any(i => i.AuthenticationType == "Google"))
        {
            TempData["Error"] = "Error loading external login information.";
            return RedirectToAction(nameof(Login));
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

        if (string.IsNullOrEmpty(email))
        {
            TempData["Error"] = "Email claim not received from provider.";
            return RedirectToAction(nameof(Login));
        }

        // Check if user already exists
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            // Auto-provision user on first external login
            user = new AppUser
            {
                Email = email,
                PasswordHash = "EXTERNAL_AUTH_NO_PASSWORD"
            };
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        // Re-issue cookie based on our internal scheme, clearing the external scheme
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Admin");
    }
}
