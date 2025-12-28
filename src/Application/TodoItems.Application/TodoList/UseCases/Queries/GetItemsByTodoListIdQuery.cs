using AutoMapper;
using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Queries;

public record GetItemsByTodoListIdQuery(Guid TodoListId) : IRequest<IAppResult>;

public class GetItemsByTodoListIdQueryHandler(ITodoListRepository repository, IMapper mapper) 
    : IRequestHandler<GetItemsByTodoListIdQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetItemsByTodoListIdQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllItemsAsync(request.TodoListId, cancellationToken);

        if(items?.Count == 0)
            throw new AppValidationsException($"No se encontraron ítems para la lista de tareas con Id {request.TodoListId}.");

        var dtos = _mapper.Map<IReadOnlyCollection<TodoItemReportDto>>(items);

        return AppResult.Success(x => x.WithData(dtos));      
    }
}
