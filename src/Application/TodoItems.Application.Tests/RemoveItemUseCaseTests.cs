using Moq;
using TodoItems.Application.UseCases;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Tests;

public class RemoveItemUseCaseTests
{
    [Fact]
    public void Execute_ShouldRemoveItem()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetById(1)).Returns(item);

        var useCase = new RemoveItemUseCase(repoMock.Object);

        useCase.Execute(1);

        repoMock.Verify(r => r.Remove(item), Times.Once);
    }
}