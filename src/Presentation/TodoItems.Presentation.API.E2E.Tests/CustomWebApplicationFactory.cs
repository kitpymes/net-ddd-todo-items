namespace TodoItems.Presentation.API.E2E.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoItems.Application.Events;
using TodoItems.Infrastructure.Events;
using TodoItems.Infrastructure.Persistence;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover DbContext real
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ItemDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Reemplazar por InMemory
            services.AddDbContext<ItemDbContext>(options =>
                options.UseInMemoryDatabase("E2E_DB"));

            // Dispatcher real
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        });
    }
}