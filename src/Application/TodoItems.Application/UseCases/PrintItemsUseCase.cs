using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.UseCases;

public class PrintItemsUseCase(IItemRepository repository)
{
    private readonly IItemRepository _repository = repository;

    public void Execute()
    {
        foreach (var item in _repository.GetAll())
        {
            Console.WriteLine($"[{item.Id}] {item.Title} - {item.Category}");
            Console.WriteLine($"    {item.Description}");

            foreach (var p in item.Progressions)
            {
                Console.WriteLine($"    -> {p.Date:d}: {p.Percent}%");
            }
        }
    }
}
