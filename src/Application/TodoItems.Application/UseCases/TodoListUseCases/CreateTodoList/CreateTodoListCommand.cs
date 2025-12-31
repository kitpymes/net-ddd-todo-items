using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoList;

public record CreateTodoListCommand(string Title, string? Description) : IRequest<IAppResult>;

public class CreateTodoListCommandHandler(ITodoListRepository repository) : IRequestHandler<CreateTodoListCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var existsTitle = await _repository.ExistsAsync(x => x.Title == request.Title, cancellationToken);

        if (existsTitle)
            throw new AppValidationsException($"Ya existe un proyecto con el mismo título: '{request.Title}'." );

        var todoList = new TodoList(request.Title, request.Description);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success(x => x.WithData(todoList.Id));
    }
}
