using FluentAssertions;
using Moq;
using TodoItems.Application.UseCases;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;
using Xunit;

namespace TodoItems.Application.Tests;

public class UpdateItemUseCaseTests
{
    [Fact]
    public void Execute_ShouldUpdateDescription()
    {
        var item = new Item(1, "Title", "Old", "Cat");

        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetById(1)).Returns(item);

        var useCase = new UpdateItemUseCase(repoMock.Object);

        useCase.Execute(1, "New");

        item.Description.Should().Be("New");
    }

    [Fact]
    public void Execute_ItemNotFound_ShouldThrow()
    {
        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetById(1)).Returns((Item?)null);

        var useCase = new UpdateItemUseCase(repoMock.Object);

        Action act = () => useCase.Execute(1, "Desc");

        act.Should().Throw<InvalidOperationException>();
    }
}
