using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.UseCases;

public class UpdateItemUseCase(IItemRepository repository)
{
    private readonly IItemRepository _repository = repository;

    public void Execute(int id, string description)
    {
        var item = _repository.GetById(id)
            ?? throw new InvalidOperationException("Item not found");

        item.UpdateDescription(description);
    }
}