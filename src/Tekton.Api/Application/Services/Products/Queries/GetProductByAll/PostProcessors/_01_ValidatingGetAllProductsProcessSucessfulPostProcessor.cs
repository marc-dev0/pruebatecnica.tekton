using MediatR.Pipeline;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Helpers;

namespace Tekton.Api.Application.Services.Products.Queries.GetProductByAll.PostProcessors;

public class _01_ValidatingGetAllProductsProcessSucessfulPostProcessor :
    IRequestPostProcessor<GetProductByAllQuery, Response<IEnumerable<GetProductByAllDto>>>
{
    private readonly IStatusCacheHelper _statusCacheHelper;
    public _01_ValidatingGetAllProductsProcessSucessfulPostProcessor(
        IStatusCacheHelper statusCacheHelper)
    {
        _statusCacheHelper = statusCacheHelper;
    }

    public async Task Process(
        GetProductByAllQuery request, 
        Response<IEnumerable<GetProductByAllDto>> response, 
        CancellationToken cancellationToken)
    {
        Log.Information("Valldating process product all list");
        if (response.Data.Count() > 0)
        {
            response.IsSuccess = true;
            response.Message = "Consulta Exitosa";
            foreach (var item in response.Data)
            {
                item.StatusName = _statusCacheHelper.GetStatusName(item.Status);
            }
        }
    }
}
