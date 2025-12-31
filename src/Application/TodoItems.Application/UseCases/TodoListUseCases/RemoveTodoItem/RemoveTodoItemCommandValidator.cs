using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.RemoveTodoItem;

public class RemoveTodoItemCommandValidator : AbstractValidator<RemoveTodoItemCommand>
{
    public RemoveTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("El Id del ítem debe ser mayor que cero.");
    }
}
