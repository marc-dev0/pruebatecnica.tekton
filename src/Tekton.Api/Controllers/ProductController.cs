using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tekton.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    [HttpGet("GetById/{productId}")]
    public async Task<IActionResult> Get(int productId)
    {
        if (productId <= 0)
        {
            return BadRequest(new { message = "Debe especificar un Id válido"});
        }

        return Ok();
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update()
    {
        return Ok();
    }
}
