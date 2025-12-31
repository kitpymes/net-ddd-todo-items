using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.RegisterProgressTodoItem;

public class RegisterProgressTodoItemCommandValidator : AbstractValidator<RegisterProgressTodoItemCommand>
{
    public RegisterProgressTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("El Id del Item es obligatorio.");

        RuleFor(p => p.Percent)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("El Porcentaje debe estar entre 0 y 100.");
    }
}