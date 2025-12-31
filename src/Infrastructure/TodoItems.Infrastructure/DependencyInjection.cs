using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Infrastructure.Persistence;
using TodoItems.Infrastructure.Persistence.Repositories;

namespace TodoItems.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection LoadPersistence(this IServiceCollection services)
    {
        services.AddScoped<ITodoListRepository, TodoListRepository>();

        var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");

        connection.Open();

        services.AddDbContext<TodoListDbContext>(options => options.UseSqlite(connection));

        var appService = services.BuildServiceProvider();

        var context = appService.GetRequiredService<TodoListDbContext>();

        context.Database.EnsureCreated();

        TodoListDataSeeder.Seed(context);

        return services;
    }
}
