using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.Title).NotEmpty().WithMessage("El título es obligatorio.");

        RuleFor(x => x.Category).NotEmpty().WithMessage("La Categoria es obligatoria.");
    }
}
