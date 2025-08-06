using Microsoft.EntityFrameworkCore;
using TableTabsApp.Models;

namespace TableTabsApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<KitchenUser> KitchenUsers { get; set; }
}
