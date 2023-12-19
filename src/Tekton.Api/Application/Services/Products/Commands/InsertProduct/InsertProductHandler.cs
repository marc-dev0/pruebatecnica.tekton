using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Commands.InsertProduct;

public class InsertProductHandler : IRequestHandler<InsertProductCommand, Response<bool>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InsertProductHandler(
        IMapper mapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<bool>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Adding new product to database : {productId}", JsonConvert.SerializeObject(request, Formatting.Indented));

        var response = new Response<bool>();
        var requestMap = _mapper.Map<Product>(request);

        _productRepository.Add(requestMap);
        response.Data = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        return response;
    }
}
