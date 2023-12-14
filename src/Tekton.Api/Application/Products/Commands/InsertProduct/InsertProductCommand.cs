using MediatR;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Products.Commands.InsertProduct;

public class InsertProductCommand : IRequest<Response<bool>>
{
    public string? Name { get; set; } 
    public bool Status { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
