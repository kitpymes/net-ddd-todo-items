using MediatR;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Commands.AddItem;

public class AddItemCommandHandler(IItemRepository repository) : IRequestHandler<AddItemCommand>
{
    private readonly IItemRepository _repository = repository;

    public Task Handle(AddItemCommand request, CancellationToken ct)
    {
        var item = new Item(
            request.Id,
            request.Title,
            request.Description,
            request.Category);

        _repository.Add(item);

        return Task.CompletedTask;
    }
}