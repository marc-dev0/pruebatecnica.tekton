using AutoMapper;
using Tekton.Api.Application.Products.Commands.InsertProduct;
using Tekton.Api.Application.Products.Queries;
using Tekton.Api.Domain;

namespace Tekton.Api.Application.Mappers;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<GetProductByIdDto, Product>().ReverseMap();
        CreateMap<InsertProductCommand, Product>().ReverseMap();
    }
}
