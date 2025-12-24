using MediatR;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Commands.RegisterProgression;

public class RegisterProgressionCommandHandler(IItemRepository repository)
        : IRequestHandler<RegisterProgressionCommand>
{
    private readonly IItemRepository _repository = repository;

    public Task Handle(RegisterProgressionCommand request, CancellationToken ct)
    {
        var item = _repository.GetById(request.ItemId)
            ?? throw new InvalidOperationException("Item not found");

        item.RegisterProgression(request.Date, request.Percent);

        return Task.CompletedTask;
    }
}
