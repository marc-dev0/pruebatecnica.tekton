using AutoMapper;
using Moq;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Application.Services.Products.Queries.GetProductByAll;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Queries.GetProductByAll;

public class GetProductByAllHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IStatusCacheHelper> _statusCacheHelperMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public GetProductByAllHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _statusCacheHelperMock = new Mock<IStatusCacheHelper>();
        _productRepositoryMock = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAllProducts()
    {
        // Arrange
        var handler = new GetProductByAllHandler(_mapperMock.Object, _statusCacheHelperMock.Object, _productRepositoryMock.Object);
        var products = new List<Product> { 
            new Product() {
                ProductId = 1,
                Name = "Producto 1",
                Status = true,
                StatusName = "Activo",
            }
        };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);
        _mapperMock.Setup(mapper => mapper.Map<List<GetProductByAllDto>>(It.IsAny<List<Product>>())).Returns(new List<GetProductByAllDto>());

        // Act
        var result = await handler.Handle(new GetProductByAllQuery(), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        _mapperMock.Verify(mapper => mapper.Map<List<GetProductByAllDto>>(It.IsAny<List<Product>>()), Times.Once);
    }
}
