using TodoItems.Application;
using TodoItems.Infrastructure;
using TodoItems.Domain;
using TodoItems.Presentation.API._Common.Middlewares;
using TodoItems.Presentation.API._Common.Extensions;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Presentation.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadPresentation();

builder.Services.LoadApplication();

builder.Services.LoadPersistence();

bool isConsole = args.Contains("--console");

if (isConsole)
{
    var repository = builder.Services.BuildServiceProvider().GetRequiredService<ITodoListRepository>();

    var todoList = await repository.GetAllTodoListAsync(CancellationToken.None);

    foreach (var list in todoList)
    {
        list.PrintItems();
    }

    return;
}

var app = builder.Build(); 

if (app.Environment.IsDevelopment()) 
{ 
    app.LoadSwagger();
}

app.UseMiddleware<AppValidationsMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

var myAllowSpecificOrigins = "_TodoItemsAngular";

app.UseCors(myAllowSpecificOrigins);

app.MapControllers();

app.Run();

public partial class Program { }

