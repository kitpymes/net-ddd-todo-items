using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.UseCases;

public class AddItemUseCase(IItemRepository repository)
{
    private readonly IItemRepository _repository = repository;

    public void Execute(int id, string title, string description, string category)
    {
        var item = new Item(id, title, description, category);
        _repository.Add(item);
    }
}
