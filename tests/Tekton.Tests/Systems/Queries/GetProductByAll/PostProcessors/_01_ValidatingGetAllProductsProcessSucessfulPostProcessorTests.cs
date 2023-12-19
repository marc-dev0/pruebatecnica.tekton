using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Application.Services.Products.Queries.GetProductByAll;
using Tekton.Api.Application.Services.Products.Queries.GetProductByAll.PostProcessors;

namespace Tekton.Tests.Systems.Queries.GetProductByAll.PostProcessors;

public class _01_ValidatingProcessSucessfulPostProcessorTests
{
    private readonly Mock<IStatusCacheHelper> _statusCacheHelperMock;
    private readonly _01_ValidatingGetAllProductsProcessSucessfulPostProcessor _processor;

    public _01_ValidatingProcessSucessfulPostProcessorTests()
    {
        _statusCacheHelperMock = new Mock<IStatusCacheHelper>();
        _processor = new _01_ValidatingGetAllProductsProcessSucessfulPostProcessor(_statusCacheHelperMock.Object);
    }

    [Fact]
    public async Task Process_WithData_SetsSuccessAndMessage()
    {
        // Arrange
        var response = new Response<IEnumerable<GetProductByAllDto>>
        {
            Data = new List<GetProductByAllDto>
            {
                new GetProductByAllDto { Status = true },
                new GetProductByAllDto { Status = false }
            }
        };
        _statusCacheHelperMock.Setup(x => x.GetStatusName(It.IsAny<bool>())).Returns("Active");

        // Act
        await _processor.Process(new GetProductByAllQuery(), response, CancellationToken.None);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal("Consulta Exitosa", response.Message);
        foreach (var item in response.Data)
        {
            Assert.Equal("Active", item.StatusName);
            _statusCacheHelperMock.Verify(x => x.GetStatusName(item.Status), Times.Once);
        }
    }

    [Fact]
    public async Task Process_WithEmptyData_DoesNotChangeResponse()
    {
        // Arrange
        var response = new Response<IEnumerable<GetProductByAllDto>> { Data = new List<GetProductByAllDto>() };

        // Act
        await _processor.Process(new GetProductByAllQuery(), response, CancellationToken.None);

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Message);
        _statusCacheHelperMock.Verify(x => x.GetStatusName(It.IsAny<bool>()), Times.Never);
    }
}
