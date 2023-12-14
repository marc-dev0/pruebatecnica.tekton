using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tekton.Api.Application.Products.Commands.InsertProduct;
using Tekton.Api.Application.Products.Queries;
using Tekton.Api.Infraestructure.Persistences;

namespace Tekton.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IMediator _mediator;

    public ProductController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> Get([FromQuery] GetProductByIdQuery query)
    {
        if (query.ProductId <= 0)
        {
            return BadRequest(new { message = "Debe especificar un Id válido"});
        }

        var response = await _mediator.Send(query);
        if (response.Data == null)
        {
            return BadRequest(new { message = "Producto no encontrado" });
        }

        return Ok(response);
    }

    [HttpPost("InsertAsync")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertProductCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update()
    {
        return Ok();
    }
}
