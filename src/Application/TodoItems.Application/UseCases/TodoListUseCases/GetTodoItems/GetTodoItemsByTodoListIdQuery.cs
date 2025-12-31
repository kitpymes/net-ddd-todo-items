using AutoMapper;
using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Application.UseCases.TodoListUseCases._Common.DTOs;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.GetTodoItems;

public record GetTodoItemsByTodoListIdQuery(Guid TodoListId) : IRequest<IAppResult>;

public class GetItemsByTodoListIdQueryHandler(ITodoListRepository repository, IMapper mapper) 
    : IRequestHandler<GetTodoItemsByTodoListIdQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetTodoItemsByTodoListIdQuery request, CancellationToken cancellationToken)
    {
        var existsTodoList = await _repository.ExistsAsync(x => x.Id == request.TodoListId, cancellationToken);

        if(!existsTodoList)
            throw new AppValidationsException($"La lista con ID '{request.TodoListId}' no existe.");

        var items = await _repository.GetAllItemsAsync(request.TodoListId, cancellationToken);

        var dtos = _mapper.Map<IReadOnlyCollection<TodoItemDto>>(items);

        return AppResult.Success(x => x.WithData(dtos));      
    }
}
