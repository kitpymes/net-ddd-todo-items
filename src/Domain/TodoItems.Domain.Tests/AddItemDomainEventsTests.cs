using FluentAssertions;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class AddItemDomainEventsTests
{
    [Fact]
    public void CreatingItem_ShouldRaise_ItemCreatedEvent()
    {
        var todoList = new TodoList();
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));

        todoList.DomainEvents.Should().ContainSingle(e => e is TodoItemCreatedEvent);
    }
}
