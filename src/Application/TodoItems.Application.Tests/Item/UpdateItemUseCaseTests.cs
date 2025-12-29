using Moq;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.Item;

public class UpdateItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldUpdateItem()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var category = new Category(Guid.NewGuid().ToString());
        var description = Guid.NewGuid().ToString();
        var newDescription = Guid.NewGuid().ToString();
        var todoList = new Domain.Aggregates.TodoListAggregate.TodoList();
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), description, category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new UpdateItemCommandHandler(repoMock.Object);

        var request = new UpdateItemCommand(todoList.Id, itemId, newDescription);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<Domain.Aggregates.TodoListAggregate.TodoList>(tl => tl.Items.Any(i => i.Description == newDescription)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
