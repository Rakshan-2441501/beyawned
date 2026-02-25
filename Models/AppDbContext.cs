using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Beyawned.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ContactSubmission> ContactSubmissions { get; set; } = null!;
    public DbSet<AppUser> AppUsers { get; set; } = null!;
}

public class ContactSubmission
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    public string Industry { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

public class AppUser
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}
