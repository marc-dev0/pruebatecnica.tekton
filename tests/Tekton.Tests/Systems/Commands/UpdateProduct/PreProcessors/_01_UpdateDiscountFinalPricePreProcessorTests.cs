using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Proxies;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct.PreProcessor;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Commands.UpdateProduct.PreProcessors;

public class _01_UpdateDiscountFinalPricePreProcessorTests
{
    public class UpdateDiscountFinalPricePreProcessorTests
    {
        [Fact]
        public async Task Process_ProductNotFound_ThrowsProductNotFoundException()
        {
            // Arrange
            var command = new UpdateProductCommand { ProductId = 1 };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var discountProviderMock = new Mock<IDiscountProvider>();
            var objectContextMock = new Mock<IObjectContext<Product>>();

            var preProcessor = new _01_UpdateDiscountFinalPricePreProcessor(
                discountProviderMock.Object,
                objectContextMock.Object,
                productRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => preProcessor.Process(command, CancellationToken.None));
        }

        [Fact]
        public async Task Process_ProductFound_CallsDoOperationsAndAddModifiedObject()
        {
            // Arrange
            var command = new UpdateProductCommand { ProductId = 1 };
            var product = new Product { ProductId = 1, Price = 100 };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var discountProviderMock = new Mock<IDiscountProvider>();
            discountProviderMock.Setup(provider => provider.GetRandomDiscount(It.IsAny<int>()))
                .ReturnsAsync(new DiscountResponse { Discount = 10 });

            var objectContextMock = new Mock<IObjectContext<Product>>();

            var preProcessor = new _01_UpdateDiscountFinalPricePreProcessor(
                discountProviderMock.Object,
                objectContextMock.Object,
                productRepositoryMock.Object);

            // Act
            await preProcessor.Process(command, CancellationToken.None);

            // Assert
            discountProviderMock.Verify(provider => provider.GetRandomDiscount(1), Times.Once);        
        }
    }
}
