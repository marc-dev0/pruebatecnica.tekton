using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Api.Application.Products.Commands.InsertProduct;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Persistences;

namespace Tekton.Tests.Systems.Commands;

public class InsertProductHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var dataContextMock = new Mock<IDataContext>();
        var mapperMock = new Mock<IMapper>();
        var validationRulesMock = new Mock<InsertProductValidator>();

        var handler = new InsertProductHandler(dataContextMock.Object, mapperMock.Object, validationRulesMock.Object);

        var validCommand = new InsertProductCommand
        {
            // Configura el objeto InsertProductCommand con datos válidos para tu prueba
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        validationRulesMock.Setup(x => x.Validate(validCommand)).Returns(validationResult);

        var product = new Product(); // Configura según tus necesidades
        mapperMock.Setup(x => x.Map<Product>(validCommand)).Returns(product);

        dataContextMock.Setup(x => x.Products.AddAsync(product, default)).Verifiable();
        dataContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var response = await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Registro exitoso");
        response.Errors.Should().BeNull();

        dataContextMock.Verify(); // Verifica que se llamó a AddAsync y SaveChangesAsync
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsErrorResponse()
    {
        // Arrange
        var dataContextMock = new Mock<IDataContext>();
        var mapperMock = new Mock<IMapper>();
        var validationRulesMock = new Mock<InsertProductValidator>();

        var handler = new InsertProductHandler(dataContextMock.Object, mapperMock.Object, validationRulesMock.Object);

        var invalidCommand = new InsertProductCommand
        {
            // Configura el objeto InsertProductCommand con datos inválidos para tu prueba
        };

        // Configura el comportamiento del Validator al invocar el método Validate
        validationRulesMock
            .Setup(x => x.Validate(It.IsAny<InsertProductCommand>()))
            .Returns(new FluentValidation.Results.ValidationResult()); // Puedes personalizar esto según sea necesario

        // Act
        var response = await handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Be("Errores de validación");
        response.Errors.Should().NotBeNull();
        response.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
    {
        // Arrange
        var dataContextMock = new Mock<IDataContext>();
        var mapperMock = new Mock<IMapper>();
        var validationRulesMock = new Mock<InsertProductValidator>();

        var handler = new InsertProductHandler(dataContextMock.Object, mapperMock.Object, validationRulesMock.Object);

        var command = new InsertProductCommand();
        validationRulesMock.Setup(x => x.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());

        dataContextMock.Setup(x => x.Products.AddAsync(It.IsAny<Product>(), default)).ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Be("Simulated exception");
        response.Errors.Should().BeNull();
    }
}
