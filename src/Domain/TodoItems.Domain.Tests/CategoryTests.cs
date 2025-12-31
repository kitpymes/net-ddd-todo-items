using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class CategoryTests
{
    [Fact]
    public void Constructor_Should_SetProperties()
    {
        var name = Guid.NewGuid().ToString();
        var progression = new Category(name);

        progression.Name.Should().Be(name);
        progression.ToString().Should().Be(name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_WhenInvalidName(string value)
    {
        Action act = () => new Category(value);

        act.Should()
           .Throw<DomainValidationException>()
           .WithMessage("El nombre de la categoría no puede estar vacío.");
    }

    [Fact]
    public void Equals_Should_BeTrue_ForSameValues()
    {
        var name = Guid.NewGuid().ToString();
        var a = new Category(name);
        var b = new Category(name);

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}