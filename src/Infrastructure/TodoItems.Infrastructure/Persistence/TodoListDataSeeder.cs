using Microsoft.EntityFrameworkCore;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Infrastructure.Persistence;

public static class TodoListDataSeeder
{
    public static void Seed(TodoListDbContext context)
    {
        if (context.TodoLists.Any()) return;

        var projectReport = new TodoList();

        var workCat = new Category("Work");
        var managementCat = new Category("Management");
        var researchCat = new Category("Research");

        projectReport.AddItem(101, "Configuración de Arquitectura", "Configurar el esqueleto de DDD y EF Core", workCat);
        projectReport.AddItem(102, "Reunión de Stakeholders", "Presentación de avances del primer sprint", managementCat);
        projectReport.AddItem(103, "Investigación de UI/UX", "Analizar tendencias de diseño para 2026", researchCat);

        context.TodoLists.Add(projectReport);

        context.SaveChanges();

        var todoList = context.TodoLists
            .Include(t => t.Items)            
                .ThenInclude(i => i.Progressions)
            .FirstOrDefault();

        todoList.RegisterProgression(101, DateTime.UtcNow.AddHours(1), 30);
        todoList.RegisterProgression(101, DateTime.UtcNow.AddHours(2), 50);
        todoList.RegisterProgression(101, DateTime.UtcNow.AddHours(3), 20);

        context.TodoLists.Update(todoList);

        context.SaveChanges();
    }
}