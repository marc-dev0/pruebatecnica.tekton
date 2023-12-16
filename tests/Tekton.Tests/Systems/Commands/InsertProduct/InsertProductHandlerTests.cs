using AutoMapper;
using Moq;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Commands.InsertProduct;

public class InsertProductHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var productRepositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new InsertProductHandler(mapperMock.Object, productRepositoryMock.Object, unitOfWorkMock.Object);

        var validCommand = new InsertProductCommand
        {
            Name = "Producto 1",
            Status = true,
            Stock = 10,
            Description = "Descripción producto 1",
            Price = 15.50M
        };

        mapperMock.Setup(x => x.Map<Product>(It.IsAny<InsertProductCommand>())).Returns(new Product());
        productRepositoryMock.Setup(x => x.Insert(It.IsAny<Product>()));
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Registro exitoso", result.Message);
        // Add more assertions based on your specific response structure

        // Verify that the repository insert and unit of work save changes were called
        productRepositoryMock.Verify(x => x.Insert(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_UnitOfWorkFails_ReturnsFailureResponse()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var productRepositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new InsertProductHandler(mapperMock.Object, productRepositoryMock.Object, unitOfWorkMock.Object);

        var validCommand = new InsertProductCommand
        {
        };

        mapperMock.Setup(x => x.Map<Product>(It.IsAny<InsertProductCommand>())).Returns(new Product());
        productRepositoryMock.Setup(x => x.Insert(It.IsAny<Product>()));
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

        // Act
        var result = await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(null, result.Message);

        productRepositoryMock.Verify(x => x.Insert(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

}
