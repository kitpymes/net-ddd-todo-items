using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class UpdateTodoListUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldUpdateTodoListTitle()
    {
        // Arrange
        var title = Guid.NewGuid().ToString();
        var newTitle = Guid.NewGuid().ToString();
        var todoList = new TodoList(title);

        var repoMock = new Mock<ITodoListRepository>();
        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new UpdateTodoListTitleCommandHandler(repoMock.Object);

        var request = new UpdateTodoListTitleCommand(todoList.Id, newTitle);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Title == newTitle),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldUpdateTodoListDescription()
    {
        // Arrange
        var description = Guid.NewGuid().ToString();
        var newDescription = Guid.NewGuid().ToString();
        var todoList = new TodoList(Guid.NewGuid().ToString(), description);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new UpdateTodoListDescriptionCommandHandler(repoMock.Object);

        var request = new UpdateTodoListDescriptionCommand(todoList.Id, newDescription);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Description == newDescription),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
