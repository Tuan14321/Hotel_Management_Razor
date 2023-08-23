using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Hotel_Management_Razor.UI.Models;

namespace Hotel_Management_Razor.UI.Data;

public class Hotel_Management_RazorUIContext : IdentityDbContext<IdentityUser>
{
    public Hotel_Management_RazorUIContext(DbContextOptions<Hotel_Management_RazorUIContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Hotel_Management_Razor.UI.Models.Company>? Company { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Room>? Room { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Department>? Department { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Floor>? Floor { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Product>? Product { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.RoomType>? RoomType { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Booking>? Booking { get; set; }

    public DbSet<Hotel_Management_Razor.UI.Models.Customer>? Customer { get; set; }
}
