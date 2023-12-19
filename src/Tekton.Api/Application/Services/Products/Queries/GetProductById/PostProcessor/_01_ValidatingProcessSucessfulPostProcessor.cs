using MediatR.Pipeline;
using Serilog;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Queries.GetProductById.PostProcessor;

public class _01_ValidatingProcessSucessfulPostProcessor : 
    IRequestPostProcessor<GetProductByIdQuery, Response<GetProductByIdDto>>
{
    public _01_ValidatingProcessSucessfulPostProcessor()
    {
    }

    public async Task Process(
        GetProductByIdQuery request, 
        Response<GetProductByIdDto> response, 
        CancellationToken cancellationToken)
    {
        Log.Information("Valldating process product by id");
        if ( response.Data is not null)  
        {
            response.IsSuccess = true;
            response.Message = "Consulta Exitosa";
        }        
    }
}
