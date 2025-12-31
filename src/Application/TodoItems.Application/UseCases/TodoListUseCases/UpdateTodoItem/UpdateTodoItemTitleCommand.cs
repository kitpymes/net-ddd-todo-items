using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoItem;

public record UpdateTodoItemTitleCommand(Guid TodoListId, int ItemId, string Title) : IRequest<IAppResult>;

public class UpdateTodoItemTitleCommandHandler(ITodoListRepository repository) : IRequestHandler<UpdateTodoItemTitleCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(UpdateTodoItemTitleCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            throw new AppValidationsException($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.UpdateItemTitle(request.ItemId, request.Title);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}
