using Moq;
using TodoItems.Application.UseCases;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Tests;

public class AddItemUseCaseTests
{
    [Fact]
    public void Execute_ShouldAddItem()
    {
        var repoMock = new Mock<IItemRepository>();

        var useCase = new AddItemUseCase(repoMock.Object);

        useCase.Execute(1, "Title", "Desc", "Cat");

        repoMock.Verify(r => r.Add(It.Is<Item>(i => i.Id == 1)), Times.Once);
    }
}