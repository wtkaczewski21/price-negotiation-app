using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder modelBuilder)
    {
        modelBuilder.Properties<decimal>().HavePrecision(10, 2);
    }

    public DbSet<Product> Products { get; set; }
}
