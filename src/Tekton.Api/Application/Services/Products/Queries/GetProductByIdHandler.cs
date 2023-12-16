using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Queries;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdDto>>
{
    //private readonly IMemoryCache _memoryCache;
    private readonly IMemoryCacheWrapper _memoryCache;
    //private readonly IDataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IStatusCacheHelper _statusCacheHelper;
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(
        //IDataContext dataContext,
        IMapper mapper,
        IMemoryCacheWrapper memoryCache,
        IStatusCacheHelper statusCacheHelper,
        IProductRepository productRepository)
    {
        //_dataContext = dataContext;
        _mapper = mapper;
        _memoryCache = memoryCache;
        _statusCacheHelper = statusCacheHelper;
        _productRepository = productRepository;
    }

    public async Task<Response<GetProductByIdDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Getting product list by ProductId : {productId}", request.ProductId);
        var cacheKey = $"Product_{request.ProductId}";
        var response = new Response<GetProductByIdDto>();

        if (_memoryCache.TryGetValue(cacheKey, out GetProductByIdDto products))
        {
            Log.Information("Products found in cache: {productId}", request.ProductId);
            response.Data = products!;
        }
        else
        {
            Log.Information("Products not found in cache. Loading them into cache: {productId}", request.ProductId);
            /*var result = await _dataContext
                           .Products
                           .FirstOrDefaultAsync(p => p.ProductId == request.ProductId);*/
            var result = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result is null)
                throw new ProductNotFoundException(request.ProductId);
            else
            {
                result.StatusName = _statusCacheHelper.GetStatusName(result.Status);
                response.Data = _mapper.Map<GetProductByIdDto>(result);
                response.IsSuccess = true;
                response.Message = "Consulta Exitosa";
            }
        }

        return response;
    }
}
