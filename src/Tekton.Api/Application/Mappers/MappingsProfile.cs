using AutoMapper;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Application.Services.Products.Queries;
using Tekton.Api.Domain;

namespace Tekton.Api.Application.Mappers;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<GetProductByIdDto, Product>().ReverseMap();
        CreateMap<InsertProductCommand, Product>().ReverseMap();

        CreateMap<UpdateProductCommand, Product>();
    }
}
