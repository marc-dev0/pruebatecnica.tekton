using Tekton.Api.Application.Services.Products.Commands.InsertProduct;

namespace Tekton.Tests.Systems.Commands.InsertProduct;

public class InsertProductValidatorTests
{
    [Fact]
    public void Validate_ValidCommand_ReturnsTrue()
    {
        // Arrange
        var validator = new InsertProductValidator();
        var validCommand = new InsertProductCommand
        {
            Name = "Product Name",
            Status = true,
            Description = "Product Description",
            Price = 10.50M
            // Set other valid properties
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
        var validator = new InsertProductValidator();
        var invalidCommand = new InsertProductCommand
        {
            Name = null,
            Status = true,
            Description = "Product Description",
            Price = 10.50M
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
        var validator = new InsertProductValidator();
        var invalidCommand = new InsertProductCommand
        {
            Name = "Product Name",
            Status = true,
            Description = null,
            Price = 10.50M
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
        var validator = new InsertProductValidator();
        var invalidCommand = new InsertProductCommand
        {
            Name = "Product Name",
            Status = true,
            Description = "Product Description",
            Price = -1
        };

        // Act
        var result = validator.Validate(invalidCommand);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("El campo Price debe ser mayor o igual a 0.", result.Errors.First().ErrorMessage);
    }
}
