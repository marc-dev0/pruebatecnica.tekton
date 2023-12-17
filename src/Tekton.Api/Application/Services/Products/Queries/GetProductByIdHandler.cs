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
    private readonly IMemoryCacheWrapper _memoryCache;
    private readonly IMapper _mapper;
    private readonly IStatusCacheHelper _statusCacheHelper;
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(
        IMapper mapper,
        IMemoryCacheWrapper memoryCache,
        IStatusCacheHelper statusCacheHelper,
        IProductRepository productRepository)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _statusCacheHelper = statusCacheHelper;
        _productRepository = productRepository;
    }

    public async Task<Response<GetProductByIdDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Getting product list by ProductId : {productId}", request.ProductId);
        var response = new Response<GetProductByIdDto>();

        Log.Information("Products not found in cache. Loading them into cache: {productId}", request.ProductId);
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

        return response;
    }
}
