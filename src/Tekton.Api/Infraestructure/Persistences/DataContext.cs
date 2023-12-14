using Microsoft.EntityFrameworkCore;
using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure.Persistences;

public class DataContext : DbContext, IDataContext
{
    public DbSet<Product> Products { get; set; }
    public DataContext(
        DbContextOptions<DataContext> options) : base(options)
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
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

}
