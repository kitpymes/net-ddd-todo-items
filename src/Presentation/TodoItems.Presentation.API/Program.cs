using Microsoft.EntityFrameworkCore;
using TodoItems.Application.Commands.AddItem;
using TodoItems.Application.Events;
using TodoItems.Domain.Interfaces;
using TodoItems.Infrastructure.Events;
using TodoItems.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddItemCommand).Assembly));

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<ItemCreatedEventHandler>();
builder.Services.AddScoped<ProgressionRegisteredEventHandler>();
builder.Services.AddDbContext<ItemDbContext>(options => options.UseInMemoryDatabase("ItemsDb"));
var app = builder.Build(); 
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); } 
app.MapControllers(); 
app.Run();

public partial class Program { }