using Serilog;
using System.Reflection;
using Tekton.Api.Application;
using Tekton.Api.Infraestructure.Repositories;
using Tekton.Api.Infraestructure.Repositories.Interfaces;
using Tekton.Api.Middleware;

namespace Tekton.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInjectionApplication(builder.Configuration);
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/logs_tekton.log", rollingInterval: RollingInterval.Day).CreateLogger();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();
            
            app.Run();
        }
    }
}
