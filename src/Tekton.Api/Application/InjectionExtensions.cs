using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tekton.Api.Application.Commons;
using Tekton.Api.Application.Helpers;
using Tekton.Api.Application.Proxies;
using Tekton.Api.Application.Services.Products;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct;
using Tekton.Api.Application.Services.Products.Commands.UpdateProduct.PreProcessor;
using Tekton.Api.Infraestructure;
using Tekton.Api.Infraestructure.Repositories;
using Tekton.Api.Infraestructure.Repositories.Interfaces;

namespace Tekton.Api.Application;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
               , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddSingleton(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IDiscountProvider, ApiDiscountProvider>();
        services.AddScoped<IProductRepository, ProductRepository>();
        //services.AddScoped<ProductRepository>();
        services.Decorate<IProductRepository, CachedProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IObjectContext<>), typeof(ObjectContext<>));
        services.AddScoped<IStatusCacheHelper, StatusCacheHelper>();
        services.AddScoped<IMemoryCacheWrapper, MemoryCacheWrapper>();
        services.AddScoped<DiscountService>();
        services.AddScoped(typeof(ObjectContext<>));

        services.AddTransient<IRequestPreProcessor<UpdateProductCommand>, _01_UpdateDiscountFinalPricePreProcessor>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        return services;
    }
}
