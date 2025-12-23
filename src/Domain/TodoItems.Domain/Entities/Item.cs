using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Events;
using TodoItems.Domain.ValueObjects;

namespace TodoItems.Domain.Entities;

public class Item
{
    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private readonly List<Progression> _progressions = [];
    public IReadOnlyCollection<Progression> Progressions => _progressions.AsReadOnly();

    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    private Item() { }

    public Item(int id, string title, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        Id = id;
        Title = title;
        Description = description;
        Category = category;

        AddDomainEvent(new ItemCreatedEvent(id));
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");

        Description = description;
    }

    public void RegisterProgression(DateTime date, decimal percent)
    {
        var progression = new Progression(date, percent);
        _progressions.Add(progression);

        AddDomainEvent(new ProgressionRegisteredEvent(Id, percent));
    }

    private void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
