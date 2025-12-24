using MediatR;

namespace TodoItems.Application.Commands.AddItem;

public record AddItemCommand(
    int Id,
    string Title,
    string Description,
    string Category
) : IRequest;
