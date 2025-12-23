using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoItems.Application.Events;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Events;
using TodoItems.Infrastructure.Events;
using TodoItems.Infrastructure.Persistence;
using Xunit;

namespace TodoItems.Infrastructure.IntegrationTests;

public class EfItemRepositoryTests
{
    [Fact]
    public async Task SavingItem_ShouldExecuteDomainEvent()
    {
        var services = new ServiceCollection();

        services.AddScoped<IDomainEventDispatcher, FakeDomainEventDispatcher>();

        services.AddDbContext<ItemDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ItemDbContext>();

        context.Items.Add(new Item(1, "Title", "Desc", "Cat"));

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task SavingItem_ShouldDispatch_ItemCreatedEvent()
    {
        var dispatcher = new FakeDomainEventDispatcher();

        var services = new ServiceCollection();
        services.AddSingleton<IDomainEventDispatcher>(dispatcher);

        services.AddDbContext<ItemDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ItemDbContext>();

        context.Items.Add(new Item(1, "Title", "Desc", "Cat"));
        await context.SaveChangesAsync();

        dispatcher.DispatchedEvents
            .Should()
            .ContainSingle(e => e is ItemCreatedEvent);
    }
}