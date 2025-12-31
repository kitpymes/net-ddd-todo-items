using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Presentation.API._Common;

/// <summary>
/// <response code="500">Error interno de la aplicación.</response>
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
public abstract class ApiControllerBase : ControllerBase
{
#nullable disable

    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

#nullable restore
}
