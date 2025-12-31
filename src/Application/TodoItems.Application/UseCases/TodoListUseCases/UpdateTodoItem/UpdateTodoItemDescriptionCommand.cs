using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoItem;

public record UpdateTodoItemDescriptionCommand(Guid TodoListId, int ItemId, string Description) : IRequest<IAppResult>;

public class UpdateTodoItemDescriptionCommandHandler(ITodoListRepository repository) : IRequestHandler<UpdateTodoItemDescriptionCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(UpdateTodoItemDescriptionCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            throw new AppValidationsException($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.UpdateItemDescription(request.ItemId, request.Description);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}
