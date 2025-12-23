using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.UseCases;

public class RemoveItemUseCase(IItemRepository repository)
{
    private readonly IItemRepository _repository = repository;

    public void Execute(int id)
    {
        var item = _repository.GetById(id)
            ?? throw new InvalidOperationException("Item not found");

        _repository.Remove(item);
    }
}
