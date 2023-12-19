using AutoMapper;
using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Application.Services.Products.Queries.GetProductById;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Queries.GetProductById;

public class GetProductByIdHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsProduct_WhenProductIsFound()
    {
        // Arrange
        var productId = 2;
        var mockMapper = new Mock<IMapper>();
        var mockStatusCacheHelper = new Mock<IStatusCacheHelper>();
        var mockProductRepository = new Mock<IProductRepository>();
        var handler = new GetProductByIdHandler(mockMapper.Object, mockStatusCacheHelper.Object, mockProductRepository.Object);
        var request = new GetProductByIdQuery { ProductId = 1 };

        var product = new Product
        {
            ProductId = productId,
            Name = "Test Product 2",
            Status = true,
            StatusName = "Active",
            Stock = 35,
            Description = "Descripcion",
            Price = 15.50M
        };
        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);
        var productDto = new GetProductByIdDto
        {
            ProductId = productId,
            Name = "Test Product 2",
            Status = true,
            StatusName = "Active",
            Stock = 35,
            Description = "Descripcion",
            Price = 15.50M
        };
        mockMapper.Setup(mapper => mapper.Map<GetProductByIdDto>(It.IsAny<Product>()))
                  .Returns(productDto);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(productDto, response.Data);
    }

    [Fact]
    public async Task Handle_ThrowsProductNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockStatusCacheHelper = new Mock<IStatusCacheHelper>();
        var mockProductRepository = new Mock<IProductRepository>();
        var handler = new GetProductByIdHandler(mockMapper.Object, mockStatusCacheHelper.Object, mockProductRepository.Object);
        var request = new GetProductByIdQuery { ProductId = 1 };

        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowProductNotFoundException_WhenRepositoryReturnsNull()
    {
        // Arrange
        var productId = 2;
        var statucCacheHelper = new Mock<IStatusCacheHelper>();
        var mockCache = new Mock<IMemoryCacheWrapper>();
        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IProductRepository>();

        mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);

        var handler = new GetProductByIdHandler(
            mockMapper.Object,
            statucCacheHelper.Object,
            mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(
            async () => await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CallsGetStatusName_WhenProductIsFound()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockStatusCacheHelper = new Mock<IStatusCacheHelper>();
        var mockProductRepository = new Mock<IProductRepository>();
        var handler = new GetProductByIdHandler(mockMapper.Object, mockStatusCacheHelper.Object, mockProductRepository.Object);
        var request = new GetProductByIdQuery { ProductId = 1 };

        var product = new Product
        {
            Name = "Test Product 2",
            Status = true,
            StatusName = "Active",
            Stock = 35,
            Description = "Descripcion",
            Price = 15.50M
        };
        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);
        var productDto = new GetProductByIdDto
        {
            Name = "Test Product 2",
            Status = true,
            StatusName = "Active",
            Stock = 35,
            Description = "Descripcion",
            Price = 15.50M
        };
        mockMapper.Setup(mapper => mapper.Map<GetProductByIdDto>(It.IsAny<Product>()))
                  .Returns(productDto);
        var statusName = "Active";
        mockStatusCacheHelper.Setup(helper => helper.GetStatusName(It.IsAny<bool>()))
                             .Returns(statusName);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockStatusCacheHelper.Verify(helper => helper.GetStatusName(product.Status), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenMapperThrowsException()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockStatusCacheHelper = new Mock<IStatusCacheHelper>();
        var mockProductRepository = new Mock<IProductRepository>();
        var handler = new GetProductByIdHandler(mockMapper.Object, mockStatusCacheHelper.Object, mockProductRepository.Object);
        var request = new GetProductByIdQuery { ProductId = 1 };

        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Product());
        mockMapper.Setup(mapper => mapper.Map<GetProductByIdDto>(It.IsAny<Product>()))
                  .Throws(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
    }
}
