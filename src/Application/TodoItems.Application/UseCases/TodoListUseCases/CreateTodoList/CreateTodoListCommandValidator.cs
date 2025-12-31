using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("El título es obligatorio.");
    }
}
