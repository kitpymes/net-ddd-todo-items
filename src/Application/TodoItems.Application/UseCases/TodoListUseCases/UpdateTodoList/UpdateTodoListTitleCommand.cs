using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;

public record UpdateTodoListTitleCommand(Guid TodoListId, string Title) : IRequest<IAppResult>;

public class UpdateTodoListTitleCommandHandler(ITodoListRepository repository) : IRequestHandler<UpdateTodoListTitleCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(UpdateTodoListTitleCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            throw new AppValidationsException($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.UpdateTitle(request.Title);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}
