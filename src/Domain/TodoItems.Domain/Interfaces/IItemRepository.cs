using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Entities;

namespace TodoItems.Domain.Interfaces;

public interface IItemRepository
{
    void Add(Item item);
    Item? GetById(int id);
    IReadOnlyCollection<Item> GetAll();
    void Remove(Item item);
}
