using Microsoft.Extensions.DependencyInjection;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Domain;

public static class DependencyInjection
{
    public static IServiceCollection LoadDomain(this IServiceCollection services)
    {
        services.AddScoped<ITodoList, TodoList>();

        return services;
    }
}
