using Microsoft.EntityFrameworkCore;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Infrastructure.Persistence.Repositories;

public class TodoListRepository(TodoListDbContext context) : ITodoListRepository
{
    private readonly TodoListDbContext _context = context;

    public async Task<int> GetNextItemIdAsync()
        => (await _context.Set<TodoItem>().MaxAsync(x => (int?)x.Id) ?? 0) + 1;

    public async Task<IReadOnlyCollection<string>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        => await _context.Set<TodoItem>()
            .Select(x => x.Category.Name)
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<TodoList>> GetAllTodoListAsync(CancellationToken cancellationToken)
        => await _context.TodoLists
            .Include(x => x.Items)
                .ThenInclude(i => i.Progressions)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<TodoList?> GetTodoListByIdAsync(Guid todoListId, CancellationToken cancellationToken)
        => await _context.TodoLists
            .Include(x => x.Items)
                .ThenInclude(i => i.Progressions)
            .FirstOrDefaultAsync(x => x.Id == todoListId, cancellationToken);

    public async Task<IReadOnlyCollection<TodoItem>> GetAllItemsAsync(Guid todoListId, CancellationToken cancellationToken)
    => await _context.TodoLists
        .Where(l => l.Id == todoListId)
        .SelectMany(l => l.Items)
            .Include(i => i.Progressions)
        .OrderBy(i => i.Id)
        .ToListAsync(cancellationToken);

    public async Task SaveAsync(TodoList todoList, CancellationToken cancellationToken)
    {
        if (_context.Entry(todoList).State == EntityState.Detached)
        {
            _context.TodoLists.Add(todoList);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
