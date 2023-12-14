using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tekton.Api.Controllers;

namespace Tekton.Tests.Systems.Controllers;

public class TestProductController
{
    [Fact]
    public async Task GetOnSucessReturnsStatusCode200()
    {
        //Arrange
        var product = new ProductController();
        int productId = 1;

        //Act
        var result = (OkResult) await product.Get(productId);

        //Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetOnFailedWhenProductIsInvalidReturnsStatusCode400()
    {
        //Arrange
        var product = new ProductController();
        int productId = 0;

        //Act
        var result = (BadRequestObjectResult) await product.Get(productId);

        //Assert
        result.StatusCode.Should().Be(400);
    }
}
