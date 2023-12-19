using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Application.Services.Products.Queries.GetProductById;
using Tekton.Api.Application.Services.Products.Queries.GetProductByAll;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Controllers;

/// <summary>
/// El controlador de producto
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de <ver cref="ProductController"/> clase.
    /// </summary>
    /// <param name="mediator"></param>
    public ProductController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene el producto especifico con el ID que envien
    /// </summary>
    /// <param name="query">El codigo del usuario enviado</param>    
    /// <returns>El producto encontrado con el ID que se envio, en caso exista</returns>
    [ProducesResponseType(typeof(Response<GetProductByIdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetById")]
    public async Task<IActionResult> Get([FromQuery] GetProductByIdQuery query)
    {
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    /// <summary>
    /// Obtiene todos los productos
    /// </summary>
    /// <param name="query"></param>    
    /// <returns>Todos los productos existentes en la bd</returns>
    [ProducesResponseType(typeof(Response<GetProductByIdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetByAll")]
    public async Task<IActionResult> Get([FromQuery] GetProductByAllQuery query)
    {
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    /// <summary>
    /// Crea un nuevo producto basado en el request enviado
    /// </summary>
    /// <param name="command">The create user request.</param>
    /// <returns>Estado si fue exitoso</returns>
    [HttpPost("InsertAsync")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status201Created)]
    public async Task<IActionResult> InsertAsync([FromBody] InsertProductCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }


    /// <summary>
    /// Actualiza el producto con el ID enviado y el request que envian
    /// </summary>
    /// <param name="command">El usuario con los datos actualizados.</param>
    /// <returns>Sin contenido.</returns>
    [HttpPut("UpdateAsync")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateProductCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
}
