using Microsoft.EntityFrameworkCore;
using System.Threading;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Infraestructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<Product>().FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
       _dbContext.Set<Product>().ToListAsync(cancellationToken);
    public void Add(Product product) => _dbContext.Set<Product>().Add(product);
    public void Update(Product product) => _dbContext.Set<Product>().Update(product);
    public async Task Delete(int productId, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Set<Product>().FindAsync(new object[] { productId }, cancellationToken);
        if (product != null)
        {
            _dbContext.Set<Product>().Remove(product);
        }
    }
}