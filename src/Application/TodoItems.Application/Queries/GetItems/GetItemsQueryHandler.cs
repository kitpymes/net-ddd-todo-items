using MediatR;
using TodoItems.Application.DTOs;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Queries.GetItems;

public class GetItemsQueryHandler
    : IRequestHandler<GetItemsQuery, IReadOnlyCollection<ItemDto>>
{
    private readonly IItemRepository _repository;

    public GetItemsQueryHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<ItemDto>> Handle(
        GetItemsQuery request,
        CancellationToken ct)
    {
        var items = _repository.GetAll()
            .Select(i => new ItemDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Category = i.Category
            })
            .ToList();

        return Task.FromResult<IReadOnlyCollection<ItemDto>>(items);
    }
}
