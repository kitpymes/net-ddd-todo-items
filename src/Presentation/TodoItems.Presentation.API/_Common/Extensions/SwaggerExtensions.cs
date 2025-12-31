using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TodoItems.Presentation.API._Common.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection LoadSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Todo List API",
                Version = "v1",
                Description = "Gestión de tareas con Clean Architecture y MediatR.",
                Contact = new OpenApiContact
                {
                    Name = "Equipo de Desarrollo",
                    Email = "dev@tuempresa.com"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static IApplicationBuilder LoadSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List API v1");
            options.DocumentTitle = "Documentación de TodoList";
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}