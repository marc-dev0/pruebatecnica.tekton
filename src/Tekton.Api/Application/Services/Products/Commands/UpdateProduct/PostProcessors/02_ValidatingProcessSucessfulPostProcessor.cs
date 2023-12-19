using MediatR.Pipeline;
using Serilog;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Commands.UpdateProduct.PostProcessors;

public class _02_ValidatingProcessSucessfulPostProcessor : IRequestPostProcessor<UpdateProductCommand, Response<bool>>
{
    public _02_ValidatingProcessSucessfulPostProcessor()
    {

    }
    public async Task Process(UpdateProductCommand request, 
        Response<bool> response, 
        CancellationToken cancellationToken)
    {
        Log.Information("Valdating result update product handler");
        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
            Log.Information("Product updated successfully");
        }
    }
}
