namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;

public record CreateTodoItemRequest(string Title, string Category, string? Description = null);