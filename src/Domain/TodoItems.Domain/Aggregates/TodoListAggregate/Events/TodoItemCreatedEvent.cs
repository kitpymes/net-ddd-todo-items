using MediatR;

namespace TodoItems.Domain.Aggregates.TodoListAggregate.Events;

public record class TodoItemCreatedEvent(Guid TodoListId, int ItemId) : INotification;