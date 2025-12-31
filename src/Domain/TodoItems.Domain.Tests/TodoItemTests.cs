using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.Entities;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class TodoItemTests
{
    [Fact]
    public void CreateItem_ShouldCreateSuccessfully()
    {
        var id = new Random().Next();
        var title = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var categoryName = Guid.NewGuid().ToString();
        var category = new Category(categoryName);

        var item = new TodoItem(id, title, category, description);

        item.Id.Should().Be(id);
        item.Title.Should().Be(title);
        item.Description.Should().Be(description);
        item.Category.Should().Be(category);
        item.Category.Name.Should().Be(categoryName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateItem_InvalidItemId_ShouldThrow(int value)
    {
        Action act = () => new TodoItem(value, Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>().WithMessage("El Id debe ser un número positivo mayor que cero.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateItem_InvalidTitle_ShouldThrow(string? value)
    {
        Action act = () => new TodoItem(new Random().Next(1, 1000), value!, new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>().WithMessage("El título es obligatorio.");
    }
}