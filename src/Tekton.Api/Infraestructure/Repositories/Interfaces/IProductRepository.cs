using Microsoft.EntityFrameworkCore;
using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Product product);
    void Update(Product product);
    Task Delete(int productId, CancellationToken cancellationToken = default);
}
