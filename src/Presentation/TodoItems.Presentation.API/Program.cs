using TodoItems.Application;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Infrastructure;
using TodoItems.Domain;
using TodoItems.Presentation.API;
using TodoItems.Presentation.API.Extensions;
using TodoItems.Presentation.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);

builder.Services.LoadPresentation();

builder.Services.LoadApplication();

builder.Services.LoadDomain();

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

app.MapControllers();

app.Run();

public partial class Program { }

