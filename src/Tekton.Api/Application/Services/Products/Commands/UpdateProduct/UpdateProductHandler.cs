using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Proxies;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Response<bool>>
{
    private readonly IMapper _mapper;
    private readonly IDiscountProvider _discountProvider;
    private readonly IObjectContext<Product> _objectContext;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductHandler(
        IMapper mapper,
        IDiscountProvider discountProvider,
        IObjectContext<Product> objectContext,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _discountProvider = discountProvider;
        _objectContext = objectContext;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<bool>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        Log.Information("Updating product from database : {productId}", JsonConvert.SerializeObject(request, Formatting.Indented));
        var response = new Response<bool>();

        Product product = GetProductRequest(request);

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        _productRepository.Update(product);
        response.Data = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        
        return response;
    }

    public Product? GetProductRequest(
        UpdateProductCommand request)
    {
        Log.Information("Creating request to send to update");
        var modifiedProduct = _objectContext.GetModifiedObject(nameof(Product));

        if (modifiedProduct is not null)
        {
            modifiedProduct.Name = request.Name;
            modifiedProduct.Status = request.Status;
            modifiedProduct.Stock = request.Stock;
            modifiedProduct.Description = request.Description;
            modifiedProduct.Price = request.Price;
        }
        
        return modifiedProduct;
    }
}
