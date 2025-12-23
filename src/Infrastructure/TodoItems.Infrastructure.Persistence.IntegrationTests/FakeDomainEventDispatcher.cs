using TodoItems.Application.Events;
using TodoItems.Domain.Events;

namespace TodoItems.Infrastructure.IntegrationTests;

public class FakeDomainEventDispatcher : IDomainEventDispatcher
{
    public List<DomainEvent> DispatchedEvents { get; } = new();

    public Task DispatchAsync(IEnumerable<DomainEvent> events)
    {
        DispatchedEvents.AddRange(events);
        return Task.CompletedTask;
    }
}
