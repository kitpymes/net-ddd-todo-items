namespace TodoItems.Application.UseCases.TodoListUseCases._Common.DTOs;

public record TodoListReportDto(List<TodoListDto> Proyectos);

public record TodoListDto(Guid Id, string Title, string? Description, List<TodoItemDto> Items);

public record TodoItemDto(
    int Id,
    string Title,
    string Category,
    string? Description,
    List<ProgressionDto> Progression
);

public record ProgressionDto(DateTime Date, decimal Percent);
