using MediatR;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Queries;

public class GetProductByIdQuery : IRequest<Response<GetProductByIdDto>>
{
    public int ProductId { get; set; }
}
