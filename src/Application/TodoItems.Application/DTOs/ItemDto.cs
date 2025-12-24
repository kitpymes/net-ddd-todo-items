namespace TodoItems.Application.DTOs;

public class ItemDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Category { get; init; } = default!;
}
