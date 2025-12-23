using FluentAssertions;
using Xunit;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Events;

namespace TodoItems.Domain.Tests;

public class ItemDomainEventsTests
{
    [Fact]
    public void CreatingItem_ShouldRaise_ItemCreatedEvent()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        item.DomainEvents.Should()
            .ContainSingle(e => e is ItemCreatedEvent);
    }

    [Fact]
    public void RegisterProgression_ShouldRaise_ProgressionRegisteredEvent()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        item.RegisterProgression(DateTime.Now, 30);

        item.DomainEvents
            .OfType<ProgressionRegisteredEvent>()
            .Should()
            .Contain(e => e.Percent == 30);
    }
}
