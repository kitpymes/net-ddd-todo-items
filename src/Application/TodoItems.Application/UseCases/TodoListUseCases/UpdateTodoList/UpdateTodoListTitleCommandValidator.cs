using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;

public class UpdateTodoListTitleCommandValidator : AbstractValidator<UpdateTodoListTitleCommand>
{
    public UpdateTodoListTitleCommandValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty().WithMessage("El Id del proyecto es obligatorio.");

        RuleFor(x => x.Title).NotEmpty().WithMessage("El título es obligatorio.");
    }
}
