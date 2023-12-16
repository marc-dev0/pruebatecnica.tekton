using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;

namespace Tekton.Tests.Systems.Commands.UpdateProduct;

public class UpdateProductValidatorTests
{
    [Fact]
    public void Validate_ValidCommand_ReturnsTrue()
    {
        // Arrange
        var validator = new UpdateProductValidator();
        var validCommand = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Product Name Updated",
            Status = true,
            Description = "Product Description Updated",
            Price = 15.50M
        };

        // Act
        var result = validator.Validate(validCommand);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidName_ReturnsFalse()
    {
        // Arrange
        var validator = new UpdateProductValidator();
        var invalidCommand = new UpdateProductCommand
        {
            ProductId = 1,
            Name = null,
            Status = true,
            Description = "Product Description Updated",
            Price = 15.50M
        };

        // Act
        var result = validator.Validate(invalidCommand);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("El nombre del producto no puede ser nulo", result.Errors.First().ErrorMessage);
    }

    [Fact]
    public void Validate_InvalidDescription_ReturnsFalse()
    {
        // Arrange
        var validator = new UpdateProductValidator();
        var invalidCommand = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Product Name Updated",
            Status = true,
            Description = null,
            Price = 15.50M
        };

        // Act
        var result = validator.Validate(invalidCommand);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("La descripción del producto no puede ser nulo", result.Errors.First().ErrorMessage);
    }

    [Fact]
    public void Validate_InvalidPrice_ReturnsFalse()
    {
        // Arrange
        var validator = new UpdateProductValidator();
        var invalidCommand = new UpdateProductCommand
        {
            ProductId = 1,
            Name = "Product Name Updated",
            Status = true,
            Description = "Product Description Updated",
            Price = -1
        };

        // Act
        var result = validator.Validate(invalidCommand);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("El campo Price debe ser mayor o igual a 0.", result.Errors.First().ErrorMessage);
    }
}
