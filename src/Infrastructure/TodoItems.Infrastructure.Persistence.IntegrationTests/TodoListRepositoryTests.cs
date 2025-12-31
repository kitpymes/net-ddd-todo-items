using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Entities;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;
using TodoItems.Infrastructure.Persistence;

namespace TodoItems.Infrastructure.IntegrationTests;

public class TodoListRepositoryTests : IDisposable
{
    private readonly TodoListDbContext _context;
    private readonly SqliteConnection _connection;

    public TodoListRepositoryTests()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(mediatorMock.Object);

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<TodoListDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TodoListDbContext(options, serviceProviderMock.Object);

        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task Save_ShouldPersistTodoListWithItemsAndProgressions()
    {
        // ARRANGE: Creamos un agregado completo con datos complejos
        var todoList = new TodoList(Guid.NewGuid().ToString());
        var itemId = 101;
        var date = new DateTime(2025, 12, 29); // Fecha de hoy en 2025

        todoList.AddItem(itemId, "Tarea Infra", new Category("Cat"), "Probar persistencia");
        todoList.RegisterItemProgression(itemId, date, 25);
        todoList.RegisterItemProgression(itemId, date.AddHours(1), 30);

        // ACT: Guardamos en la base de datos
        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync();

        // Limpiamos el tracking de EF para forzar una lectura real de la DB
        _context.ChangeTracker.Clear();

        // ASSERT: Recuperamos y verificamos la integridad del grafo de objetos
        var persistedList = await _context.TodoLists
            .Include(x => x.Items)
                .ThenInclude(i => i.Progressions)
            .FirstOrDefaultAsync(x => x.Id == todoList.Id);

        persistedList.Should().NotBeNull();
        persistedList!.Items.Should().HaveCount(1);

        var persistedItem = persistedList.Items.First();
        persistedItem.Id.Should().Be(itemId);

        // Verificamos que los Value Objects (Progressions) se hayan guardado correctamente
        persistedItem.Progressions.Should().HaveCount(2);
        persistedItem.Progressions.Sum(p => p.Percent).Should().Be(55);
    }

    [Fact]
    public async Task Update_ShouldPersistChangesInPrivateCollection()
    {
        // ARRANGE: Partimos de un estado guardado
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(1, "Original", new Category("Cat"), "Desc");
        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // ACT: Cargamos, modificamos usando lógica de dominio y volvemos a guardar
        var listToUpdate = await _context.TodoLists.Include(x => x.Items).FirstAsync();
        listToUpdate.UpdateItemDescription(1, "Nueva Descripción 2025");
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // ASSERT: Verificamos la actualización
        var updatedList = await _context.TodoLists.Include(x => x.Items).FirstAsync();
        updatedList.Items.First().Description.Should().Be("Nueva Descripción 2025");
    }

    [Fact]
    public async Task Delete_ShouldRemoveAggregateAndItsChildren()
    {
        // ARRANGE
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(1, "A borrar", new Category("Cat"), "D");
        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync();

        // ACT
        _context.TodoLists.Remove(todoList);
        await _context.SaveChangesAsync();

        // ASSERT: Verificamos borrado en cascada (si está configurado)
        var listCount = await _context.TodoLists.CountAsync();
        var itemCount = await _context.Set<TodoItem>().CountAsync();

        listCount.Should().Be(0);
        itemCount.Should().Be(0, "porque el borrado del agregado debe eliminar sus componentes internos");
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
