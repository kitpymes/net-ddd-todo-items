using FluentValidation;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
           .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("El Id del ítem debe ser mayor que cero.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.");
    }
}
