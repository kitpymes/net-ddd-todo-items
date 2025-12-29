using Moq;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.Item;

public class RemoveItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRemoveItem()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var category = new Category(Guid.NewGuid().ToString());
        var todoList = new Domain.Aggregates.TodoListAggregate.TodoList();
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new RemoveItemCommandHandler(repoMock.Object);

        var request = new RemoveItemCommand(todoList.Id, itemId);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<Domain.Aggregates.TodoListAggregate.TodoList>(tl => tl.Items.All(i => i.Id != itemId)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
