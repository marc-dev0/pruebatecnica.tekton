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
using Tekton.Api.Infraestructure.Persistences;
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

        //services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<IConfiguration>(configuration);

        // Fluent Validation Configurations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // DI MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // Auto Mapper Configurations
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //services.AddScoped<IDataContext, DataContext>();

        services.AddScoped<IDiscountProvider, ApiDiscountProvider>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IObjectContext<>), typeof(ObjectContext<>));
        //services.AddSingleton<StatusCacheHelper>();
        services.AddScoped<IStatusCacheHelper, StatusCacheHelper>();
        services.AddScoped<IMemoryCacheWrapper, MemoryCacheWrapper>();

        services.AddHttpClient();

        services.AddScoped<DiscountService>();
        services.AddScoped(typeof(ObjectContext<>));
        services.AddMemoryCache();
        services.AddTransient<IRequestPreProcessor<UpdateProductCommand>, _01_UpdateDiscountFinalPricePreProcessor>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        return services;
    }
}
