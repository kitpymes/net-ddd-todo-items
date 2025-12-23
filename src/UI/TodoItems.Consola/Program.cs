using TodoItems.Application.UseCases;
using TodoItems.Infrastructure.Persistence;

var repository = new InMemoryItemRepository();

var addItem = new AddItemUseCase(repository);
var updateItem = new UpdateItemUseCase(repository);
var registerProgression = new RegisterProgressionUseCase(repository);
var printItems = new PrintItemsUseCase(repository);
var removeItem = new RemoveItemUseCase(repository);

// Add
addItem.Execute(1, "DDD Course", "Learn DDD properly", "Education");

// Update
updateItem.Execute(1, "Learn DDD with real examples");

// Progress
registerProgression.Execute(1, DateTime.Now, 25);
registerProgression.Execute(1, DateTime.Now.AddDays(1), 50);

// Print
printItems.Execute();

// Remove
removeItem.Execute(1);
