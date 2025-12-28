using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoItems.Application._Common.Extensions;

namespace TodoItems.Application;

public static class DependencyInjection
{
    public static IServiceCollection LoadApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddFluentValidation(Assembly.GetExecutingAssembly());

        services.AddMediatR(config => {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
