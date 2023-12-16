using MediatR.Pipeline;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Proxies;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Commands.UpdateProduct.PreProcessor;

public class _01_UpdateDiscountFinalPricePreProcessor : IRequestPreProcessor<UpdateProductCommand>
{
    //private readonly IDataContext _dataContext;
    private readonly IDiscountProvider _discountProvider;
    private readonly IObjectContext<Product> _objectContext;
    private readonly IProductRepository _productRepository;

    public _01_UpdateDiscountFinalPricePreProcessor(
        //IDataContext dataContext,
        IDiscountProvider discountProvider,
        IObjectContext<Product> objectContext,
        IProductRepository productRepository)
    {
        //_dataContext = dataContext;
        _discountProvider = discountProvider;
        _objectContext = objectContext;
        _productRepository = productRepository;
    }

    public async Task Process(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        //var product = _dataContext.Products.FirstOrDefault(x => x.ProductId == command.ProductId);
        if (product == null)
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        await DoOperations(command, product);
        _objectContext.AddModifiedObject(nameof(Product), product);
    }
    private async Task DoOperations(
        UpdateProductCommand command,
        Product product)
    {

        var discountResponse = await _discountProvider.GetRandomDiscount(command.ProductId);
        product.ProductId = command.ProductId;
        product.Discount = discountResponse.Discount;
        product.FinalPrice = product.Price * (100 - product.Discount) / 100;
    }
}
