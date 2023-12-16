using MediatR;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Response<bool>>
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public bool Status { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
