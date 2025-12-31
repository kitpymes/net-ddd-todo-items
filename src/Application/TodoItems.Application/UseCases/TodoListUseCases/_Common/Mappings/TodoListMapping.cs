using AutoMapper;
using TodoItems.Application.UseCases.TodoListUseCases._Common.DTOs;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Entities;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.UseCases.TodoListUseCases._Common.Mappings;

public class TodoListMapping : Profile
{
    public TodoListMapping()
    {
        CreateMap<Progression, ProgressionDto>()
          .ConstructUsing(src => new ProgressionDto(src.Date, src.Percent));

        CreateMap<TodoItem, TodoItemDto>()
            .ConstructUsing((src, context) => new TodoItemDto(
                src.Id,
                src.Title,
                src.Category.Name,
                src.Description,
                context.Mapper.Map<List<ProgressionDto>>(
                    src.Progressions.OrderByDescending(p => p.Date)
                )
            ));

        CreateMap<TodoList, TodoListDto>()
            .ConstructUsing((src, context) => new TodoListDto(
                src.Id,
                src.Title,
                src.Description,
                context.Mapper.Map<List<TodoItemDto>>(
                    src.Items.OrderByDescending(p => p.Id)
                )
            ));
    }
}
