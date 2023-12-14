using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Persistences;

namespace Tekton.Api.Application.Products.Commands.InsertProduct;

public class InsertProductHandler : IRequestHandler<InsertProductCommand, Response<bool>>
{
    private readonly IDataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly InsertProductValidator _validationRules;

    public InsertProductHandler(
        IDataContext dataContext,
        IMapper mapper,
        InsertProductValidator validationRules)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _validationRules = validationRules;
    }

    public async Task<Response<bool>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Adding new product to database : {productId}", JsonConvert.SerializeObject(request, Formatting.Indented));

        var response = new Response<bool>();
        try
        {
            var validationResult = _validationRules.Validate(request);

            if (!validationResult.IsValid)
            {
                response.Message = "Errores de validación";
                response.Errors = validationResult.Errors.ToList();
                Log.Information(response.Message);
                return response;
            }
            var requestMap = _mapper.Map<Product> (request);
            await _dataContext.Products.AddAsync(requestMap);
            int result = await _dataContext.SaveChangesAsync(cancellationToken);
            if (result > 0) 
            { 
                response.IsSuccess = true;
                response.Message = "Registro exitoso";
                Log.Information("Product added successfully");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.InnerException.Message);
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
