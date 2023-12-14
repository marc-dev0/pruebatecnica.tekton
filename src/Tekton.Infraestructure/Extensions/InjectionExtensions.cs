using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tekton.Infraestructure.Persistences.Models;

namespace Tekton.Infraestructure.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Agregar configuración de EF Core SQL Server
        services.AddSqlServer<Context>(configuration.GetConnectionString("DefaultConnection"));

        // Registrar el Contexto 
        services.AddScoped<Context>();
        return services;
    }

    public static IServiceCollection AddSqlServer<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        services.AddScoped<DbContextOptions<TContext>>(provider =>
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(connectionString)
                .Options;
        });

        return services;
    }
}
