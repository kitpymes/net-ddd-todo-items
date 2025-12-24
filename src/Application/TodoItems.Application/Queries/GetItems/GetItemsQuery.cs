using MediatR;
using TodoItems.Application.DTOs;

namespace TodoItems.Application.Queries.GetItems;

public record GetItemsQuery() : IRequest<IReadOnlyCollection<ItemDto>>;
