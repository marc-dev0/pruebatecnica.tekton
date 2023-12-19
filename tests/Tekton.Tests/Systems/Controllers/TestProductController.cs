using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Application.Services.Products.Queries.GetProductById;
using Tekton.Api.Controllers;

namespace Tekton.Tests.Systems.Controllers;

public class TestProductController
{
    [Fact]
    public async Task Get_ProductById_ReturnsOkResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);
        var query = new GetProductByIdQuery { ProductId = 1 };

        mediatorMock.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(new Response<GetProductByIdDto> { Data = new GetProductByIdDto() });

        // Act
        var result = await controller.Get(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

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

    [Fact]
    public async Task InsertAsync_ValidCommand_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        var validCommand = new InsertProductCommand
        {
            Name = "Producto 1",
            Status = true,
            Stock = 10,
            Description = "Descripción producto 1",
            Price = 15.50M
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
    public async Task UpdateAsync_ValidCommand_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProductController(mediatorMock.Object);

        var validCommand = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Producto 1 update",
            Status = true,
            Stock = 20,
            Description = "Descripción producto 1 update",
            Price = 25.50M
        };

        var successResponse = new Response<bool>
        {
            IsSuccess = true,
            Message = "Actualización exitosa"
        };

        mediatorMock.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), default))
            .ReturnsAsync(successResponse);

        // Act
        var result = await controller.UpdateAsync(validCommand);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Response<bool>>(okResult.Value);

        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Actualización exitosa");

        mediatorMock.Verify(x => x.Send(validCommand, default), Times.Once);
    }
}
