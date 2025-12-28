namespace TodoItems.Application.TodoList.DTOs;

public record TodoItemReportDto(
    int Id,
    string Title,
    string Description,
    string Category,
    List<ProgressionDto> History
);
