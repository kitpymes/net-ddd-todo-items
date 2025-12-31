using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Aggregates.TodoListAggregate;

public interface ITodoList
{
    void UpdateTitle(string title);

    void UpdateDescription(string description);

    void AddItem(int id, string title, Category category, string? description);

    void UpdateItemTitle(int id, string title);

    void UpdateItemDescription(int id, string description);

    void RemoveItem(int id);

    void RegisterItemProgression(int id, DateTime dateTime, decimal percent);

    void PrintItems();
}
