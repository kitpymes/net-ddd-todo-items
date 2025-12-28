using AutoMapper;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.TodoList.Mappings;

public class ItemMapping : Profile
{
    public ItemMapping()
    {
        CreateMap<Progression, ProgressionDto>()
          .ConstructUsing(src => new ProgressionDto(src.Date, src.Percent));

        CreateMap<TodoItem, TodoItemReportDto>()
            .ConstructUsing((src, context) => new TodoItemReportDto(
                src.Id,
                src.Title,
                src.Description,
                src.Category.Name,
                context.Mapper.Map<List<ProgressionDto>>(
                    src.Progressions.OrderByDescending(p => p.Date)
                )
            ));
    }
}
