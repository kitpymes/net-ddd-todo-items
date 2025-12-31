using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoItem;

public class UpdateTodoItemDescriptionCommandValidator : AbstractValidator<UpdateTodoItemDescriptionCommand>
{
    public UpdateTodoItemDescriptionCommandValidator()
    {
        RuleFor(x => x.TodoListId)
           .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("El Id del ítem debe ser mayor que cero.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.");
    }
}
