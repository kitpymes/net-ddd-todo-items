using System.Linq.Expressions;
using TodoItems.Domain.Aggregates.TodoListAggregate.Entities;

namespace TodoItems.Domain.Aggregates.TodoListAggregate;

public interface ITodoListRepository
{
    Task<int> GetNextItemIdAsync();

    Task<IReadOnlyCollection<string>> GetAllCategoriesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TodoList>> GetAllTodoListAsync(CancellationToken cancellationToken);

    Task<TodoList?> GetTodoListByIdAsync(Guid todoListId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TodoItem>> GetAllItemsAsync(Guid todoListId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Expression<Func<TodoList, bool>> predicate, CancellationToken cancellationToken);

    Task SaveAsync(TodoList todoList, CancellationToken cancellationToken);
}
