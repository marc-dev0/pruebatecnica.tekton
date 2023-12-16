using Microsoft.EntityFrameworkCore;
using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure.Persistences;

public interface IDataContext
{
    DbSet<Product> Products { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> UpdateProductAsync(Product product);
}
