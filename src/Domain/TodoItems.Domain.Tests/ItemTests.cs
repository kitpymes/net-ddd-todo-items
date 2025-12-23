using FluentAssertions;
using Xunit;
using TodoItems.Domain.Entities;

namespace TodoItems.Domain.Tests;

public class ItemTests
{
    [Fact]
    public void CreateItem_ShouldCreateSuccessfully()
    {
        var item = new Item(1, "Title", "Description", "Category");

        item.Id.Should().Be(1);
        item.Title.Should().Be("Title");
        item.Description.Should().Be("Description");
        item.Category.Should().Be("Category");
    }

    [Fact]
    public void CreateItem_WithoutTitle_ShouldThrow()
    {
        Action act = () => new Item(1, "", "Desc", "Cat");

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Title*");
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var item = new Item(1, "Title", "Old", "Cat");

        item.UpdateDescription("New");

        item.Description.Should().Be("New");
    }

    [Fact]
    public void RegisterProgression_ShouldAddProgression()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        item.RegisterProgression(DateTime.Today, 50);

        item.Progressions.Should().HaveCount(1);
    }

    [Fact]
    public void RegisterProgression_WithInvalidPercent_ShouldThrow()
    {
        var item = new Item(1, "Title", "Desc", "Cat");

        Action act = () => item.RegisterProgression(DateTime.Now, 150);

        act.Should().Throw<ArgumentException>();
    }
}