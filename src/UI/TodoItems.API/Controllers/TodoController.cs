using Microsoft.AspNetCore.Mvc; 
using TodoItems.Domain.Interfaces; 
namespace TodoItems.API.Controllers; 
[ApiController] 
[Route("api/[controller]")] 
public class TodoController : ControllerBase { 
    //private readonly ITodoList _service; 
    //public TodoController(ITodoList service) => _service = service; 
    //[HttpPost] public IActionResult Add(int id, string title, string desc, string cat) { _service.AddItem(id, title, desc, cat); return Ok(); } 
    //[HttpGet] public IActionResult Get() => Ok(_service.GetItems()); 
    //[HttpPut("{id}")] public IActionResult Update(int id, string desc) { _service.UpdateItem(id, desc); return Ok(); } 
    //[HttpDelete("{id}")] public IActionResult Delete(int id) { _service.RemoveItem(id); return Ok(); } 
} 
