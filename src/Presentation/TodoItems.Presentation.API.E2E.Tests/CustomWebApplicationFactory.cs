using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TodoItems.Infrastructure.Persistence;

namespace TodoItems.Presentation.API.E2E.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //services.LoadPresentation();

            var dbContextConfigDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<TodoListDbContext>));
            if (dbContextConfigDescriptor != null) services.Remove(dbContextConfigDescriptor);

            var connectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(System.Data.Common.DbConnection));
            if (connectionDescriptor != null) services.Remove(connectionDescriptor);

            services.AddDbContext<TodoListDbContext>(options => options.UseInMemoryDatabase("E2E_DB"));            

            var appService = services.BuildServiceProvider();

            var context = appService.GetRequiredService<TodoListDbContext>();

            context.Database.EnsureCreated();

            TodoListDataSeeder.Seed(context);           
        });
    }
}