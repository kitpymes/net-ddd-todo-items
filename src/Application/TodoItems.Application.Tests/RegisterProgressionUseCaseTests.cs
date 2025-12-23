using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using TodoItems.Application.UseCases;
using TodoItems.Domain.Entities;
using TodoItems.Domain.Interfaces;

namespace TodoItems.Application.Tests;

public class RegisterProgressionUseCaseTests
{
    [Fact]
    public void Execute_ShouldRegisterProgression()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetById(1)).Returns(item);

        var useCase = new RegisterProgressionUseCase(repoMock.Object);

        useCase.Execute(1, DateTime.Now, 25);

        item.Progressions.Should().HaveCount(1);
    }
}
