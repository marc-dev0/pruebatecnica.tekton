using Tekton.Api.Application.Services.Products.Queries.GetProductById.PostProcessor;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Services.Products.Queries.GetProductById;

namespace Tekton.Tests.Systems.Queries.GetProductById.PostProccessor;

public class _01_ValidatingProcessSucessfulPostProcessorTests
{
    [Fact]
    public async Task Process_ShouldMarkResponseAsSuccess()
    {
        // Arrange
        var processor = new _01_ValidatingProcessSucessfulPostProcessor();
        var request = new GetProductByIdQuery { ProductId = 1 };
        var response = new Response<GetProductByIdDto>();
        response.Data = new GetProductByIdDto
        {
            ProductId = 1,
            Name = "Test",
            Status = true,
            StatusName = "Activo",
            Stock = 15,
            Description = "Descripcion test",
            Price = 15,
            Discount = 5,
            FinalPrice = 10
        };
        // Act
        await processor.Process(request, response, CancellationToken.None);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal("Consulta Exitosa", response.Message);
    }
}
