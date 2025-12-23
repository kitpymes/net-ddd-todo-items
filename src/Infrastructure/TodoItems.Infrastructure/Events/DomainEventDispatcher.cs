using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Application.Events;
using TodoItems.Domain.Events;

namespace TodoItems.Infrastructure.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ItemCreatedEventHandler _itemCreatedHandler;
    private readonly ProgressionRegisteredEventHandler _progressionHandler;

    public DomainEventDispatcher(
        ItemCreatedEventHandler itemCreatedHandler,
        ProgressionRegisteredEventHandler progressionHandler)
    {
        _itemCreatedHandler = itemCreatedHandler;
        _progressionHandler = progressionHandler;
    }

    public async Task DispatchAsync(IEnumerable<DomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            switch (domainEvent)
            {
                case ItemCreatedEvent e:
                    await _itemCreatedHandler.Handle(e);
                    break;

                case ProgressionRegisteredEvent e:
                    await _progressionHandler.Handle(e);
                    break;
            }
        }
    }
}
