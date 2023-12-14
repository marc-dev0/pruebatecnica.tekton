using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tekton.Infraestructure.Persistences.Models;

public class Context : DbContext
{
    public DbSet<Product> Products { get; set; }
    public Context(
        DbContextOptions<Context> options) : base(options)
    {

    }
}
