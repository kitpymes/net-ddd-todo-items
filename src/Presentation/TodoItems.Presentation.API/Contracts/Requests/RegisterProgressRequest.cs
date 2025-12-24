namespace TodoItems.Presentation.API.Contracts.Requests;

public record RegisterProgressRequest(
    DateTime Date,
    decimal Percent);
