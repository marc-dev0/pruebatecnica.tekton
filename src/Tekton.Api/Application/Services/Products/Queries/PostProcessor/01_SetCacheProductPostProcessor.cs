using MediatR.Pipeline;
using Microsoft.Extensions.Caching.Memory;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Queries.PostProcessor;

public class _01_SetCacheProductPostProcessor : IRequestPostProcessor<GetProductByIdQuery, Response<GetProductByIdDto>>
{
    private readonly IMemoryCacheWrapper _memoryCache;
    public _01_SetCacheProductPostProcessor(
      IMemoryCacheWrapper memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task Process(GetProductByIdQuery request, Response<GetProductByIdDto> response, CancellationToken cancellationToken)
    {
        var cacheKey = $"Product_{request.ProductId}";
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(300))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
            .SetPriority(CacheItemPriority.Normal);

        _memoryCache.Set(cacheKey, response.Data, cacheEntryOptions);
    }
}
