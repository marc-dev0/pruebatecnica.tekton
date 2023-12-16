using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Tekton.Api.Application.Commons;
using Tekton.Api.Domain;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application.Services.Products.Commands.InsertProduct;

public class InsertProductHandler : IRequestHandler<InsertProductCommand, Response<bool>>
{
    //private readonly IDataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InsertProductHandler(
        //IDataContext dataContext,
        IMapper mapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        //_dataContext = dataContext;
        _mapper = mapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<bool>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Adding new product to database : {productId}", JsonConvert.SerializeObject(request, Formatting.Indented));

        var response = new Response<bool>();

        var requestMap = _mapper.Map<Product>(request);

        _productRepository.Insert(requestMap);
        /*await _dataContext.Products.AddAsync(requestMap);
        int result = await _dataContext.SaveChangesAsync(cancellationToken);*/
        int result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            response.IsSuccess = true;
            response.Message = "Registro exitoso";
            Log.Information("Product added successfully");
        }

        return response;
    }
}
