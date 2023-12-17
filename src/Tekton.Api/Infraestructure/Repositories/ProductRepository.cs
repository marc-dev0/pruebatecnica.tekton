using Microsoft.EntityFrameworkCore;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Infraestructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<Product>().FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

    public void Add(Product product) => _dbContext.Set<Product>().Add(product);
    public void Update(Product product) => _dbContext.Set<Product>().Update(product);
}