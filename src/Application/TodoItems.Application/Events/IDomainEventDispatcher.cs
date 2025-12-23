using TodoItems.Domain.Events;

namespace TodoItems.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> events);
}