using MediatR.Pipeline;
using Serilog;
using Tekton.Api.Application.Commons;

namespace Tekton.Api.Application.Services.Products.Commands.InsertProduct.PostProcessors;

public class _01_ValidatingProcessSucessfullPostProcessor : IRequestPostProcessor<InsertProductCommand, Response<bool>>
{
    public _01_ValidatingProcessSucessfullPostProcessor()
    {

    }

    public async Task Process(InsertProductCommand request, Response<bool> response, CancellationToken cancellationToken)
    {
        Log.Information("Valdating result insert product handler");
        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = "Registro exitoso";
            Log.Information("Product added successfully");
        }
    }
}
