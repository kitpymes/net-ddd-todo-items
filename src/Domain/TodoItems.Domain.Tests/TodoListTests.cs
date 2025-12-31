using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Events;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class TodoListTests
{
    private readonly TodoList _todoList = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

    #region Tests de TodoList

    [Fact]
    public void CreateTodoList_ShouldCreateSuccessfully()
    {
        var title = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        var todoList = new TodoList(title, description);

        todoList.Title.Should().Be(title);
        todoList.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateTodoList_InvalidTitle_ShouldThrow(string? value)
    {
        Action act = () => new TodoList(value!);

        act.Should().Throw<DomainValidationException>().WithMessage("El título es obligatorio.");
    }

    [Fact]
    public void UpdateTitle_ShouldChangeTitle()
    {
        var title = Guid.NewGuid().ToString();
        var newTitle = Guid.NewGuid().ToString();

        var item = new TodoList(title);

        item.UpdateTitle(newTitle);

        item.Title.Should().Be(newTitle);
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var description = Guid.NewGuid().ToString();
        var newDescription = Guid.NewGuid().ToString();

        var item = new TodoList(Guid.NewGuid().ToString(), description);

        item.UpdateDescription(newDescription);

        item.Description.Should().Be(newDescription);
    }

    #endregion

    #region Tests de Gestión de Items

    [Fact]
    public void AddItem_ShouldAddTodoItemAndRaiseDomainEvent_WhenDataIsValid()
    {
        // Arrange
        int itemId = 1;
        string title = "Tarea de Prueba";
        string description = "Descripción detallada";
        var category = new Category("Trabajo");

        // Act
        _todoList.AddItem(itemId, title, category, description);

        // Assert
        _todoList.Items.Should().HaveCount(1);
        _todoList.Items.First().Id.Should().Be(itemId);
        _todoList.DomainEvents.Should().ContainItemsAssignableTo<TodoItemCreatedEvent>();

        var @event = _todoList.DomainEvents.OfType<TodoItemCreatedEvent>().First();
        @event.ItemId.Should().Be(itemId);
    }

    [Fact]
    public void AddItem_ShouldThrowDomainValidationException_WhenIdAlreadyExists()
    {
        // Arrange
        _todoList.AddItem(1, "Original", new Category("Trabajo"), "Desc");

        // Act
        Action act = () => _todoList.AddItem(1, "Duplicado", new Category("Trabajo"), "Desc");

        // Assert
        act.Should().Throw<DomainValidationException>().WithMessage("*ya existe en la lista*");
    }

    [Fact]
    public void RemoveItem_ShouldThrowException_WhenItemHasMoreThan50PercentProgress()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", new Category("Trabajo"), "Desc");
        _todoList.RegisterItemProgression(itemId, DateTime.Now, 51); // 51% > 50%

        // Act
        Action act = () => _todoList.RemoveItem(itemId);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*más del 50% realizado*");
    }


    [Fact]
    public void UpdateItemTitleShouldUpdateTitle_WhenProgressIsBelowOrEqual50()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", new Category("Trabajo"), "Original");
        _todoList.RegisterItemProgression(itemId, DateTime.Now, 30);
        string newTitle = "Nuevo título";

        // Act
        _todoList.UpdateItemTitle(itemId, newTitle);

        // Assert
        _todoList.Items.First(x => x.Id == itemId).Title.Should().Be(newTitle);
    }

    [Fact]
    public void UpdateItemDescription_ShouldUpdateDescription_WhenProgressIsBelowOrEqual50()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", new Category("Trabajo"), "Original");
        _todoList.RegisterItemProgression(itemId, DateTime.Now, 30);
        string newDescription = "Nueva Descripción";

        // Act
        _todoList.UpdateItemDescription(itemId, newDescription);

        // Assert
        _todoList.Items.First(x => x.Id == itemId).Description.Should().Be(newDescription);
    }

    #endregion

    #region Tests de Progresión (Reglas de Negocio)

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void RegisterProgression_ShouldThrowException_WhenPercentIsInvalid(decimal invalidPercent)
    {
        // Arrange
        _todoList.AddItem(1, "Tarea", new Category("Trabajo"), "Desc");

        // Act
        Action act = () => _todoList.RegisterItemProgression(1, DateTime.Now, invalidPercent);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*más grande de cero y menor a 100*");
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenTotalSumExceeds100()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", new Category("Trabajo"), "Desc");
        _todoList.RegisterItemProgression(itemId, DateTime.Now, 60);

        // Act
        Action act = () => _todoList.RegisterItemProgression(itemId, DateTime.Now.AddHours(1), 41);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*excede el 100%*");
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenDateIsPriorToLastProgression()
    {
        // Arrange
        int itemId = 1;
        var now = DateTime.Now;
        _todoList.AddItem(itemId, "Tarea", new Category("Trabajo"), "Desc");
        _todoList.RegisterItemProgression(itemId, now, 10);

        // Act
        // Intentamos registrar un progreso con fecha de ayer
        Action act = () => _todoList.RegisterItemProgression(itemId, now.AddDays(-1), 10);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*fecha anterior al último progreso registrado*");
    }

    #endregion

    #region Tests de Utilidades de Eventos

    [Fact]
    public void ClearDomainEvents_ShouldEmptyTheCollection()
    {
        // Arrange
        _todoList.AddItem(1, "T", new Category("Trabajo"), "D");
        _todoList.DomainEvents.Should().NotBeEmpty();

        // Act
        _todoList.ClearDomainEvents();

        // Assert
        _todoList.DomainEvents.Should().BeEmpty();
    }

    #endregion
}
