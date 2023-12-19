using MediatR;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Queries.GetProductByAll;

public class GetProductByAllQuery : IRequest<Response<IEnumerable<GetProductByAllDto>>>
{
}
