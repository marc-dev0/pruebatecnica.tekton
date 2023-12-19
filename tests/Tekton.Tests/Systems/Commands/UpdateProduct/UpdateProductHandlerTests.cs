using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Proxies;
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
    public async Task Handle_ReturnsTrue_WhenProductIsUpdatedSuccessfully()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockDiscountProvider = new Mock<IDiscountProvider>();
        var mockObjectContext = new Mock<IObjectContext<Product>>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var handler = new UpdateProductHandler(mockMapper.Object, mockDiscountProvider.Object, mockObjectContext.Object, mockProductRepository.Object, mockUnitOfWork.Object);
        var request = new UpdateProductCommand { 
            ProductId = 1,
            Name = "Nombre producto actualizado",
            Status = true,
            Stock = 40,
            Description = "Descripción actualizada",
            Price = 50
        };

        var product = new Product {
            ProductId = 1,
            Name = "Nombre producto actualizado",
            Status = true,
            Stock = 40,
            Description = "Descripción actualizada",
            Price = 50
        };

        mockObjectContext.Setup(context => context.GetModifiedObject(nameof(Product)))
                         .Returns(product);
        mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Data);
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
        };

        objectContextMock.Setup(context => context.GetModifiedObject(nameof(Product)))
            .Returns((Product)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenDatabaseUpdateFails()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockDiscountProvider = new Mock<IDiscountProvider>();
        var mockObjectContext = new Mock<IObjectContext<Product>>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var handler = new UpdateProductHandler(mockMapper.Object, mockDiscountProvider.Object, mockObjectContext.Object, mockProductRepository.Object, mockUnitOfWork.Object);
        var request = new UpdateProductCommand { };

        var product = new Product { };
        mockObjectContext.Setup(context => context.GetModifiedObject(nameof(Product)))
                         .Returns(product);
        mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(0); 

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Data);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockDiscountProvider = new Mock<IDiscountProvider>();
        var mockObjectContext = new Mock<IObjectContext<Product>>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var handler = new UpdateProductHandler(mockMapper.Object, mockDiscountProvider.Object, mockObjectContext.Object, mockProductRepository.Object, mockUnitOfWork.Object);
        var request = new UpdateProductCommand {
            ProductId = 1,
            Name = "Nombre producto actualizado",
            Status = true,
            Stock = 40,
            Description = "Descripción actualizada",
            Price = 50
        };

        mockObjectContext.Setup(context => context.GetModifiedObject(nameof(Product)))
                         .Throws(new Exception("Error inesperado"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CallsUpdateOnProductRepository_WhenProductIsFound()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockDiscountProvider = new Mock<IDiscountProvider>();
        var mockObjectContext = new Mock<IObjectContext<Product>>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var handler = new UpdateProductHandler(mockMapper.Object, mockDiscountProvider.Object, mockObjectContext.Object, mockProductRepository.Object, mockUnitOfWork.Object);
        var request = new UpdateProductCommand {
            ProductId = 1,
            Name = "Nombre producto actualizado",
            Status = true,
            Stock = 40,
            Description = "Descripción actualizada",
            Price = 50
        };

        var product = new Product {
            ProductId = 1,
            Name = "Nombre producto actualizado",
            Status = true,
            Stock = 40,
            Description = "Descripción actualizada",
            Price = 50
        };
        mockObjectContext.Setup(context => context.GetModifiedObject(nameof(Product)))
                         .Returns(product);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockProductRepository.Verify(repo => repo.Update(product), Times.Once);
    }
}
