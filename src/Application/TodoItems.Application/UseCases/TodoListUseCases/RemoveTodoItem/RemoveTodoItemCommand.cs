using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.RemoveTodoItem;

public record RemoveTodoItemCommand(Guid TodoListId, int ItemId) : IRequest<IAppResult>;

public class RemoveTodoItemCommandHandler(ITodoListRepository repository) : IRequestHandler<RemoveTodoItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(RemoveTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            throw new AppValidationsException($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.RemoveItem(request.ItemId);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}