using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Application.TodoList.UseCases.Queries;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Presentation.API.Controllers;

[ApiVersion("1.0")]
public class TodoListController : ApiControllerBase
{
    /// <summary>
    /// Obtiene todas las listas de tareas disponibles.
    /// </summary>
    /// <remarks>
    /// Este endpoint recupera una colección global de todas las carpetas o contenedores de tareas (Todo Lists). 
    /// Es el punto de partida para la navegación principal de la aplicación.
    /// </remarks>
    /// <response code="200">Retorna la lista de colecciones encontrada exitosamente.</response>
    /// <response code="500">Error interno al procesar la consulta en la base de datos.</response>
    [HttpGet]
    public async Task<IAppResult> GetAllTodoList()
        => await Mediator.Send(new GetTodoListQuery());

    /// <summary>
    /// Lista todas las categorías disponibles para clasificar tareas.
    /// </summary>
    /// <remarks>
    /// Útil para poblar componentes desplegables (dropdowns) al crear o editar una tarea.
    /// </remarks>
    /// <response code="200">Retorna el catálogo maestro de categorías (ej: Trabajo, Personal, Urgente).</response>
    [HttpGet("categories")]
    public async Task<IAppResult> GetAllCategories()
        => await Mediator.Send(new GetCategoriesQuery());

    /// <summary>
    /// Recupera todos los ítems pertenecientes a una lista específica.
    /// </summary>
    /// <param name="todoListId">El identificador único (GUID) de la lista de tareas.</param>
    /// <remarks>
    /// Utilice este endpoint cuando el usuario seleccione una lista específica para ver sus tareas pendientes y completadas.
    /// </remarks>
    /// <response code="200">Retorna los ítems vinculados al ID de la lista proporcionado.</response>
    /// <response code="400">Si el formato del GUID es inválido o la lista no existe.</response>
    [HttpGet("{todoListId}/items")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> GetAllItems(Guid todoListId)
        => await Mediator.Send(new GetItemsByTodoListIdQuery(todoListId));

    /// <summary>
    /// Crea una nueva tarea dentro de una lista específica.
    /// </summary>
    /// <param name="todoListId">ID de la lista donde se alojará el ítem.</param>
    /// <param name="request">Objeto con el título, descripción y categoría de la nueva tarea.</param>
    /// <remarks>
    /// El cuerpo de la petición debe incluir obligatoriamente un título. La categoría debe coincidir con un ID de categoría válido.
    /// </remarks>
    /// <response code="200">Tarea creada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos o IDs no encontrados.</response>
    [HttpPost("{todoListId}/item")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> AddItem(Guid todoListId, [FromBody] AddItemRequest request)
        => await Mediator.Send(new AddItemCommand(todoListId, request.Title, request.Description, request.Category));

    /// <summary>
    /// Actualiza la información de una tarea existente.
    /// </summary>
    /// <param name="todoListId">ID de la lista contenedora.</param>
    /// <param name="itemId">ID numérico de la tarea a modificar.</param>
    /// <param name="request">Contenido actualizado (principalmente la descripción).</param>
    /// <response code="200">Cambios guardados correctamente.</response>
    /// <response code="400">El ítem no pertenece a la lista indicada o los datos son incorrectos.</response>
    [HttpPut("{todoListId}/item/{itemId}")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> UpdateItem(Guid todoListId, int itemId, [FromBody] UpdateItemRequest request)
        => await Mediator.Send(new UpdateItemCommand(todoListId, itemId, request.Description));

    /// <summary>
    /// Elimina de forma permanente una tarea.
    /// </summary>
    /// <param name="todoListId">ID de la lista contenedora.</param>
    /// <param name="itemId">ID del ítem a eliminar.</param>
    /// <response code="200">Ítem borrado con éxito.</response>
    /// <response code="400">No se pudo encontrar el ítem para eliminar.</response>
    [HttpDelete("{todoListId}/item/{itemId}")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> RemoveItem(Guid todoListId, int itemId)
        => await Mediator.Send(new RemoveItemCommand(todoListId, itemId));

    /// <summary>
    /// Registra el porcentaje de avance de una tarea específica.
    /// </summary>
    /// <param name="todoListId">ID de la lista contenedora.</param>
    /// <param name="itemId">ID de la tarea.</param>
    /// <param name="request">Porcentaje de progreso (0 a 100).</param>
    /// <remarks>
    /// Permite el seguimiento granular de tareas complejas. Si el porcentaje llega a 100, la tarea se considera finalizada.
    /// </remarks>
    /// <response code="200">Progreso actualizado correctamente.</response>
    /// <response code="400">El valor del porcentaje está fuera del rango permitido.</response>
    [HttpPost("{todoListId}/item/{itemId}/progression")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> RegisterProgressItem(Guid todoListId, int itemId, [FromBody] RegisterProgressItemRequest request)
        => await Mediator.Send(new RegisterProgressItemCommand(todoListId, itemId, request.Percent));
}
