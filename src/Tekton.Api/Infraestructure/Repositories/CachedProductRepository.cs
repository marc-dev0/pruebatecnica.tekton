using Microsoft.Extensions.Caching.Memory;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Infraestructure.Repositories;

public class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedProductRepository(
        IProductRepository decorated,
        IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        string key = $"product-{productId}";
        return _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return _decorated.GetByIdAsync(productId, cancellationToken);
            });
    }

    public void Add(Product product) => _decorated.Add(product);

    public void Update(Product product) => _decorated.Update(product);
}
