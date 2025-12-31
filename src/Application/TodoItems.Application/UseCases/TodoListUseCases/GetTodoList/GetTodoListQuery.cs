using AutoMapper;
using MediatR;
using TodoItems.Application.UseCases.TodoListUseCases._Common.DTOs;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.UseCases.TodoListUseCases.GetTodoList;

public record GetTodoListQuery() : IRequest<IAppResult>;

public class GetTodoListQueryHandler(ITodoListRepository repository, IMapper mapper) : IRequestHandler<GetTodoListQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    public async Task<IAppResult> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
    {
        var proyectos = await _repository.GetAllTodoListAsync(cancellationToken);

        var dtos = _mapper.Map<IReadOnlyCollection<TodoListDto>>(proyectos);

        return AppResult.Success(x => x.WithData(dtos));
    }
}
