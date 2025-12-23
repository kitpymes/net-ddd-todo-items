using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Infrastructure.Persistence;

public class EfItemRepository(ItemDbContext context) : IItemRepository
{
    private readonly ItemDbContext _context = context;

    public void Add(Item item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();
    }

    public Item? GetById(int id)
    {
        return _context.Items
            .Include(i => i.Progressions)
            .FirstOrDefault(i => i.Id == id);
    }

    public IReadOnlyCollection<Item> GetAll()
    {
        return _context.Items.ToList();
    }

    public void Remove(Item item)
    {
        _context.Items.Remove(item);
        _context.SaveChanges();
    }
}
