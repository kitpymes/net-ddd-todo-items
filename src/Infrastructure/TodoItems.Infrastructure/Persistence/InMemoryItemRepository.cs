using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Infrastructure.Persistence;

public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> _items = [];

    public void Add(Item item) => _items.Add(item);

    public Item? GetById(int id) =>
        _items.FirstOrDefault(x => x.Id == id);

    public IReadOnlyCollection<Item> GetAll() =>
        _items.AsReadOnly();

    public void Remove(Item item) =>
        _items.Remove(item);
}