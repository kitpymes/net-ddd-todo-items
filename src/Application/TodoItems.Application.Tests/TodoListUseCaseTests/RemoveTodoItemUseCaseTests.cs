using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.RemoveTodoItem;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class RemoveTodoItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRemoveTodoItem()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var category = new Category(Guid.NewGuid().ToString());
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new RemoveTodoItemCommandHandler(repoMock.Object);

        var request = new RemoveTodoItemCommand(todoList.Id, itemId);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Items.All(i => i.Id != itemId)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
