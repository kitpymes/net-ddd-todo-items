using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;

public class UpdateTodoListDescriptionCommandValidator : AbstractValidator<UpdateTodoListDescriptionCommand>
{
    public UpdateTodoListDescriptionCommandValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty().WithMessage("El Id del proyecto es obligatorio.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("La descripción es obligatoria.");
    }
}
