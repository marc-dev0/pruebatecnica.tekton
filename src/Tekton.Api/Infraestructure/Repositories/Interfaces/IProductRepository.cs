using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    void Insert(Product product);
    void Update(Product product);
}
