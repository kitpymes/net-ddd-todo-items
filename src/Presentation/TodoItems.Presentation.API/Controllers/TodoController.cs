using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoItems.Application.Commands.AddItem;
using TodoItems.Application.Commands.RegisterProgression;
using TodoItems.Application.Queries.GetItems;
using TodoItems.Application.UseCases;
using TodoItems.Presentation.API.Contracts.Requests;

namespace TodoItems.Presentation.API.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Add(AddItemRequest request)
    {
        await _mediator.Send(new AddItemCommand(
            request.Id,
            request.Title,
            request.Description,
            request.Category));

        return Ok();
    }

    [HttpPost("{id}/progress")]
    public async Task<IActionResult> RegisterProgress(
        int id,
        RegisterProgressRequest request)
    {
        await _mediator.Send(new RegisterProgressionCommand(
            id,
            request.Date,
            request.Percent));

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetItemsQuery());
        return Ok(result);
    }
}
