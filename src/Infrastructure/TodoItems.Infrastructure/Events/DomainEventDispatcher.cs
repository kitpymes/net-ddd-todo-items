using MediatR;
using TodoItems.Application.Events;
using TodoItems.Domain.Events;

namespace TodoItems.Infrastructure.Events;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    private readonly IMediator _mediator = mediator;

    public async Task DispatchAsync(IEnumerable<DomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
