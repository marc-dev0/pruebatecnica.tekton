using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Products.Commands.InsertProduct;
using Tekton.Api.Application.Products.Queries;
using Tekton.Api.Controllers;

namespace Tekton.Tests.Systems.Controllers;

public class TestProductController
{
    [Fact]
    public async Task Get_ProductExists_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<GetProductByIdDto> { Data = new GetProductByIdDto() });

        var controller = new ProductController(mediatorMock.Object);

        // Act
        var result = await controller.Get(new GetProductByIdQuery { ProductId = 1 });

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Get_InvalidProductId_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        // Act
        var result = await controller.Get(new GetProductByIdQuery { ProductId = 0 });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        // Convert anonymous types to JSON strings for comparison
        var expectedJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { message = "Debe especificar un Id válido" });
        var actualJson = Newtonsoft.Json.JsonConvert.SerializeObject(badRequestResult.Value);

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task Get_ProductNotFound_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<GetProductByIdDto> { Data = null });

        var controller = new ProductController(mediatorMock.Object);

        // Act
        var result = await controller.Get(new GetProductByIdQuery { ProductId = 1 });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        // Convert anonymous types to JSON strings for comparison
        var expectedJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { message = "Producto no encontrado" });
        var actualJson = Newtonsoft.Json.JsonConvert.SerializeObject(badRequestResult.Value);

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task Update_ValidRequest_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        // Act
        var result = await controller.Update();

        // Assert
        var okResult = Assert.IsType<OkResult>(result);

    }

    [Fact]
    public async Task InsertAsync_ValidCommand_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        var validCommand = new InsertProductCommand
        {
            // Configura el objeto InsertProductCommand con datos válidos para tu prueba
        };

        var successResponse = new Response<bool>
        {
            IsSuccess = true,
            Message = "Registro exitoso"
        };

        mediatorMock.Setup(x => x.Send(It.IsAny<InsertProductCommand>(), default))
            .ReturnsAsync(successResponse);

        // Act
        var result = await controller.InsertAsync(validCommand);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Response<bool>>(okResult.Value);

        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Registro exitoso");

        mediatorMock.Verify(x => x.Send(validCommand, default), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_InvalidCommand_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        var invalidCommand = new InsertProductCommand
        {
            Name = null,
            Status = false,
            Stock = 0,
            Description = null,
            Price = 0
        };

        var failureResponse = new Response<bool>
        {
            IsSuccess = false,
            Message = "Error en la solicitud"
        };

        mediatorMock.Setup(x => x.Send(It.IsAny<InsertProductCommand>(), default))
            .ReturnsAsync(failureResponse);

        // Act
        var result = await controller.InsertAsync(invalidCommand);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<Response<bool>>(badRequestResult.Value);

        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Be("Error en la solicitud");

        mediatorMock.Verify(x => x.Send(invalidCommand, default), Times.Once);
    }
}
