using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class TodoListTests
{
    private readonly TodoList _todoList = new();

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
        _todoList.AddItem(itemId, title, description, category);

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
        _todoList.AddItem(1, "Original", "Desc", new Category("Trabajo"));

        // Act
        Action act = () => _todoList.AddItem(1, "Duplicado", "Desc", new Category("Trabajo"));

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*ya existe en la lista*");
    }

    [Fact]
    public void RemoveItem_ShouldThrowException_WhenItemHasMoreThan50PercentProgress()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", "Desc", new Category("Trabajo"));
        _todoList.RegisterProgression(itemId, DateTime.Now, 51); // 51% > 50%

        // Act
        Action act = () => _todoList.RemoveItem(itemId);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*más del 50% realizado*");
    }

    [Fact]
    public void UpdateItem_ShouldUpdateDescription_WhenProgressIsBelowOrEqual50()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", "Original", new Category("Trabajo"));
        _todoList.RegisterProgression(itemId, DateTime.Now, 30);
        string newDescription = "Nueva Descripción";

        // Act
        _todoList.UpdateItem(itemId, newDescription);

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
        _todoList.AddItem(1, "Tarea", "Desc", new Category("Trabajo"));

        // Act
        Action act = () => _todoList.RegisterProgression(1, DateTime.Now, invalidPercent);

        // Assert
        act.Should().Throw<DomainValidationException>()
           .WithMessage("*más grande de cero y menor a 100*");
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenTotalSumExceeds100()
    {
        // Arrange
        int itemId = 1;
        _todoList.AddItem(itemId, "Tarea", "Desc", new Category("Trabajo"));
        _todoList.RegisterProgression(itemId, DateTime.Now, 60);

        // Act
        Action act = () => _todoList.RegisterProgression(itemId, DateTime.Now.AddHours(1), 41);

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
        _todoList.AddItem(itemId, "Tarea", "Desc", new Category("Trabajo"));
        _todoList.RegisterProgression(itemId, now, 10);

        // Act
        // Intentamos registrar un progreso con fecha de ayer
        Action act = () => _todoList.RegisterProgression(itemId, now.AddDays(-1), 10);

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
        _todoList.AddItem(1, "T", "D", new Category("Trabajo"));
        _todoList.DomainEvents.Should().NotBeEmpty();

        // Act
        _todoList.ClearDomainEvents();

        // Assert
        _todoList.DomainEvents.Should().BeEmpty();
    }

    #endregion
}
