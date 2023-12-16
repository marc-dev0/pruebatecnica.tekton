using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Proxies;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct.PreProcessor;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Tekton.Tests.Systems.Commands.UpdateProduct;

public class UpdateProductHandlerTests
{
    [Fact]
    public async Task Handle_ProductFound_ReturnsSuccessResponse()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var objectContextMock = new Mock<IObjectContext<Product>>();
        var discountProviderMock = new Mock<IDiscountProvider>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<UpdateProductHandler>>();

        var handler = new UpdateProductHandler(
            mapperMock.Object,
            discountProviderMock.Object,
            objectContextMock.Object,
            productRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Updated Product",
            // Set other properties
        };

        var existingProduct = new Product { ProductId = 1, Name = "Existing Product" };

        objectContextMock.Setup(context => context.GetModifiedObject(nameof(Product)))
            .Returns(existingProduct);

        productRepositoryMock.Setup(repo => repo.Update(It.IsAny<Product>()));
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync(default(CancellationToken)))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Actualización exitosa");

        // Additional verifications
        productRepositoryMock.Verify(repo => repo.Update(existingProduct), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(default(CancellationToken)), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var objectContextMock = new Mock<IObjectContext<Product>>();
        var discountProviderMock = new Mock<IDiscountProvider>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<UpdateProductHandler>>();

        var handler = new UpdateProductHandler(
            mapperMock.Object,
            discountProviderMock.Object,
            objectContextMock.Object,
            productRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Updated Product",
            // Set other properties
        };

        objectContextMock.Setup(context => context.GetModifiedObject(nameof(Product)))
            .Returns((Product)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }
}
