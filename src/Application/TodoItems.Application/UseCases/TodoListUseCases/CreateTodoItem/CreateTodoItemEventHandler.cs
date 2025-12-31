
using MediatR;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Events;

namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;

public class CreateTodoItemEventHandler(ITodoListRepository repository) : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(notification.TodoListId, cancellationToken);

        if (todoList is not null)
        {
            todoList.RegisterItemProgression(notification.ItemId, DateTime.UtcNow, 0);

            await _repository.SaveAsync(todoList, cancellationToken);
        }        
    }
}
