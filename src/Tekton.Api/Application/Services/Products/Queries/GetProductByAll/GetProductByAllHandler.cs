using AutoMapper;
using MediatR;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Queries.GetProductByAll;

public class GetProductByAllHandler : IRequestHandler<GetProductByAllQuery, Response<IEnumerable<GetProductByAllDto>>>
{
    private readonly IMapper _mapper;
    private readonly IStatusCacheHelper _statusCacheHelper;
    private readonly IProductRepository _productRepository;

    public GetProductByAllHandler(
        IMapper mapper,
        IStatusCacheHelper statusCacheHelper,
        IProductRepository productRepository)
    {
        _mapper = mapper;
        _statusCacheHelper = statusCacheHelper;
        _productRepository = productRepository;
    }

    public async Task<Response<IEnumerable<GetProductByAllDto>>> Handle(GetProductByAllQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Getting product all list");
        var response = new Response<IEnumerable<GetProductByAllDto>>();

        var result = await _productRepository.GetAllAsync(cancellationToken);

        response.Data = _mapper.Map<List<GetProductByAllDto>>(result);

        return response;
    }
}