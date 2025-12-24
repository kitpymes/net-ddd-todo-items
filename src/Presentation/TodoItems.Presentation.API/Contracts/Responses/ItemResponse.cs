namespace TodoItems.Presentation.API.Contracts.Responses;

public record ItemResponse(
    int Id,
    string Title,
    string Description,
    string Category);