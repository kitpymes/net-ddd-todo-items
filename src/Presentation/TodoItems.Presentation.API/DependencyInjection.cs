using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;
using System.Text.RegularExpressions;
using TodoItems.Presentation.API.Extensions;

namespace TodoItems.Presentation.API;

public static class DependencyInjection
{
    public static IServiceCollection LoadPresentation(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
        });

        services.AddEndpointsApiExplorer();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
        });

        services.LoadSwagger();

        return services;
    }
}

public class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;

        return Regex.Replace(value.ToString()!, "([a-z0-9])([A-Z])", "$1-$2").ToLower();
    }
}
