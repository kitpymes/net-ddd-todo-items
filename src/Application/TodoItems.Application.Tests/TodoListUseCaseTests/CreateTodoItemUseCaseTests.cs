using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class CreateTodoItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldCreateTodoItem()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var newItemId = itemId + 1;
        var category = new Category(Guid.NewGuid().ToString());
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), category, Guid.NewGuid().ToString());

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        repoMock.Setup(r => r.GetAllCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList.Items
            .Select(x => x.Category.Name)
            .Distinct()
            .ToList()
            .AsReadOnly());

        repoMock.Setup(r => r.GetNextItemIdAsync()).ReturnsAsync(newItemId); 

        var useCase = new CreateTodoItemCommandHandler(repoMock.Object);

        var request = new CreateTodoItemCommand(todoList.Id, Guid.NewGuid().ToString(), category.Name, Guid.NewGuid().ToString());

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Items.Any(i => i.Id == newItemId)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
