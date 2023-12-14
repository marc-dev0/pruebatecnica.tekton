using FluentValidation;
using MediatR;
using System.Reflection;
using Tekton.Api.Infraestructure.Persistences;

namespace Tekton.Api.Application;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfiguration>(configuration);

        // Fluent Validation Configurations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // DI MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // Auto Mapper Configurations
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IDataContext, DataContext>();
        return services;
    }
}
