using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Infrastructure.Persistence.Configurations;

public sealed class TodoListEntityConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);

        builder.Property(t => t.Title).IsRequired();

        builder.Metadata.FindNavigation(nameof(TodoList.Items))?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey("TodoListId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}

