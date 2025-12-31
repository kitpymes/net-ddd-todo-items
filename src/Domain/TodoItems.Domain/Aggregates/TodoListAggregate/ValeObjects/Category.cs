using TodoItems.Domain._Common.Exceptions;

namespace TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

public class Category : IEquatable<Category>
{
    public string Name { get; init; } = string.Empty;

    private Category() { }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("El nombre de la categoría no puede estar vacío.");

        Name = name;
    }

    public override string ToString() => Name;

    public bool Equals(Category? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Name == other.Name;
    }

    public override bool Equals(object? obj) => Equals(obj as Category);

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(Category? left, Category? right) =>
        ReferenceEquals(left, right) || left is not null && left.Equals(right);

    public static bool operator !=(Category? left, Category? right) => !(left == right);
}
