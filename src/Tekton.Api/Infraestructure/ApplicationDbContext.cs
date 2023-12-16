using Microsoft.EntityFrameworkCore;
using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Product>()
            .Property(p => p.FinalPrice)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Product>()
            .Property(p => p.Discount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();
    }
}