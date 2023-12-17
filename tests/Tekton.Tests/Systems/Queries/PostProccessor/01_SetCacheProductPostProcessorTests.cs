using Microsoft.Extensions.Caching.Memory;
using Moq;
using Tekton.Api.Application.Services.Products.Queries.PostProcessor;
using Tekton.Api.Application.Services.Products.Queries;
using Tekton.Api.Application.Commons;

namespace Tekton.Tests.Systems.Queries.PostProccessor;

public class _01_SetCacheProductPostProcessorTests
{
    [Fact]
    public void Process_ShouldSetCacheWithCorrectKeyAndValue()
    {
        // Arrange
        var productId = 1;
        var response = new Response<GetProductByIdDto>
        {
            Data = new GetProductByIdDto { ProductId = productId, Name = "Test Product" }
        };

        var memoryCacheMock = new Mock<IMemoryCacheWrapper>();
        var postProcessor = new _01_SetCacheProductPostProcessor(memoryCacheMock.Object);

        // Act
        postProcessor.Process(new GetProductByIdQuery { ProductId = productId }, response, default);

        // Assert
        var expectedCacheKey = $"Product_{productId}";
        memoryCacheMock.Verify(c => c.Set(expectedCacheKey, response.Data, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }
}
