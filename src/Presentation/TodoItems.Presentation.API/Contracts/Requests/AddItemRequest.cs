namespace TodoItems.Presentation.API.Contracts.Requests;

public record AddItemRequest(
    int Id,
    string Title,
    string Description,
    string Category);
