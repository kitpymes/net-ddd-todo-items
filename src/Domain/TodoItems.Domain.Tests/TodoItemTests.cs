using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate;
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

        var item = new TodoItem(id, title, description, category);

        item.Id.Should().Be(id);
        item.Title.Should().Be(title);
        item.Description.Should().Be(description);
        item.Category.Should().Be(category);
        item.Category.Name.Should().Be(categoryName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateItem_InvalidTitle_ShouldThrow(string? value)
    {
        Action act = () => new TodoItem(new Random().Next(1, 1000), value!, Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>().WithMessage("El título es obligatorio.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateItem_InvalidItemId_ShouldThrow(int value)
    {
        Action act = () => new TodoItem(value, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>().WithMessage("El Id debe ser un número positivo mayor que cero.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateItem_InvalidDescription_ShouldThrow(string? value)
    {
        Action act = () => new TodoItem(new Random().Next(), Guid.NewGuid().ToString(), value, new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>().WithMessage("La descripción es obligatoria.");
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var item = new TodoItem(new Random().Next(), Guid.NewGuid().ToString(), "Old", new Category(Guid.NewGuid().ToString()));

        item.UpdateDescription("New");

        item.Description.Should().Be("New");
    }

    [Fact]
    public void AddProgression_ShouldRegisterProgression()
    {
        var percent = 25;
        var item = new TodoItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));
        var progession = new Progression(DateTime.UtcNow, percent);

        item.AddProgression(progession);

        item.Progressions.Should().HaveCount(1);
        item.Progressions.Should().Contain(progession);
        item.TotalProgress.Should().Be(percent);
        item.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void AddProgression_WithInvalidPercent_ShouldThrow()
    {
        var percent = 101;
        var item = new TodoItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new Category(Guid.NewGuid().ToString()));

        Action act = () => item.AddProgression(new Progression(DateTime.UtcNow, percent));

        act.Should().Throw<DomainValidationException>();
    }
}