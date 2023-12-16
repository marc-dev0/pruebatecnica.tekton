using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Exceptions;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Application.Services.Products.Queries;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Tests.Systems.Queries;

public class GetProductByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCachedProduct_WhenInCache()
    {
        // Arrange
        var productId = 2;
        var statucCacheHelper = new Mock<IStatusCacheHelper>();
        var mockCache = new Mock<IMemoryCacheWrapper>();
        var mockMapper = new Mock<IMapper>();

        var cachedProduct = new GetProductByIdDto { ProductId = productId, Name = "Cached Product", StatusName = "Active" };
        mockCache.Setup(c => c.TryGetValue($"Product_{productId}", out cachedProduct)).Returns(true);

        var handler = new GetProductByIdHandler(
            mockMapper.Object,
            mockCache.Object,
            statucCacheHelper.Object,
            Mock.Of<IProductRepository>());

        // Act
        var response = await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull().And.BeEquivalentTo(cachedProduct);
        mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<GetProductByIdDto>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldFetchProductFromRepository_WhenCacheMiss()
    {
        // Arrange
        var productId = 2;
        Product product = new Product { ProductId = productId, Name = "Test Product 2", Status = true };
        var statucCacheHelper = new Mock<IStatusCacheHelper>();
        var mockCache = new Mock<IMemoryCacheWrapper>();
        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IProductRepository>();
        var mapperConfig = new MapperConfiguration(mc => { mc.CreateMap<Product, GetProductByIdDto>(); });
        IMapper mockmapper = mapperConfig.CreateMapper();

        statucCacheHelper.Setup(x => x.GetStatusName(It.IsAny<bool>())).Returns("Activo");
        mockCache.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<GetProductByIdDto>(), It.IsAny<MemoryCacheEntryOptions>()))
            .Callback<string, GetProductByIdDto>((key, cache) =>
            {
                mockCache.Setup(s => s.TryGetValue(key, out cache))
                    .Returns(false);
            });
        
        mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        var handler = new GetProductByIdHandler(
            mockmapper,
            mockCache.Object,
            statucCacheHelper.Object,
            mockRepository.Object);

        // Act
        var response = await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<GetProductByIdDto>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never());
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
            mockCache.Object,
            statucCacheHelper.Object,
            mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(
            async () => await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None));
    }

}
