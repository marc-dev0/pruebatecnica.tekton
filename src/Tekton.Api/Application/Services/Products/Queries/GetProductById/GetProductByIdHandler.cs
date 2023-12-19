using AutoMapper;
using MediatR;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdDto>>
{
    private readonly IMapper _mapper;
    private readonly IStatusCacheHelper _statusCacheHelper;
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(
        IMapper mapper,
        IStatusCacheHelper statusCacheHelper,
        IProductRepository productRepository)
    {
        _mapper = mapper;
        _statusCacheHelper = statusCacheHelper;
        _productRepository = productRepository;
    }

    public async Task<Response<GetProductByIdDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Getting product list by ProductId : {productId}", request.ProductId);
        var response = new Response<GetProductByIdDto>();

        var result = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (result is null)
            throw new ProductNotFoundException(request.ProductId);

        response.Data = _mapper.Map<GetProductByIdDto>(result);
        response.Data.StatusName = _statusCacheHelper.GetStatusName(result.Status);

        return response;
    }
}
