using MediatR;

namespace TodoItems.Application.Commands.RegisterProgression;

public record RegisterProgressionCommand(
    int ItemId,
    DateTime Date,
    decimal Percent
) : IRequest;