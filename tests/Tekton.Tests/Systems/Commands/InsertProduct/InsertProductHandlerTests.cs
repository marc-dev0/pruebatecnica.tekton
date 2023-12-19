using AutoMapper;
using Moq;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Commands.InsertProduct;

public class InsertProductHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public InsertProductHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var handler = new InsertProductHandler(_mapperMock.Object, _productRepositoryMock.Object, _unitOfWorkMock.Object);
        var command = new InsertProductCommand {
            Name = "Producto 1",
            Status = true,
            Stock = 10,
            Description = "Descripción producto 1",
            Price = 15.50M
        };
        _mapperMock.Setup(m => m.Map<Product>(It.IsAny<InsertProductCommand>())).Returns(new Product());
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Handle_SaveChangesFails_ShouldReturnFailure()
    {
        // Arrange
        var handler = new InsertProductHandler(_mapperMock.Object, _productRepositoryMock.Object, _unitOfWorkMock.Object);
        var command = new InsertProductCommand {
            Name = "Producto 1",
            Status = true,
            Stock = 10,
            Description = "Descripción producto 1",
            Price = 15.50M
        };
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        // Act
        var result = await handler.Handle(command, new CancellationToken());

        // Assert
        Assert.False(result.Data);
    }


    [Fact]
    public async Task Handle_WhenExceptionThrownByDependency_ShouldHandleException()
    {
        // Arrange
        var handler = new InsertProductHandler(_mapperMock.Object, _productRepositoryMock.Object, _unitOfWorkMock.Object);
        var command = new InsertProductCommand {
            Name = "Producto 1",
            Status = true,
            Stock = 10,
            Description = "Descripción producto 1",
            Price = 15.50M
        };
        _mapperMock.Setup(m => m.Map<Product>(It.IsAny<InsertProductCommand>())).Throws(new System.Exception());

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, new CancellationToken()));
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
        productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()));
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

        // Act
        var result = await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(null, result.Message);

        productRepositoryMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

}
