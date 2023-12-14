using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Infraestructure.Persistences;

namespace Tekton.Api.Application.Products.Queries;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdDto>>
{
    private readonly IDataContext _dataContext;
    private readonly IMapper _mapper;    
    public GetProductByIdHandler(
        IDataContext dataContext,
        IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<Response<GetProductByIdDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Getting product list by ProductId : {productId}", request.ProductId);

        var response = new Response<GetProductByIdDto>();
        
        try
        {
            var result = await _dataContext
                                .Products
                                .FirstOrDefaultAsync(p => p.ProductId == request.ProductId);
            
            if (result is not null)
            {
                response.Data = _mapper.Map<GetProductByIdDto>(result);
                response.IsSuccess = true;
                response.Message = "Consulta Exitosa";
            }
        } 
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        
        return response;
    }
}
