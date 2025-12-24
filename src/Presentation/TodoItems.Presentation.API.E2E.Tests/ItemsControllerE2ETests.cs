using System.Net.Http.Json;

namespace TodoItems.Presentation.API.E2E.Tests;

public class ItemsControllerE2ETests(CustomWebApplicationFactory factory)
        : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task FullFlow_ShouldCreateItem_And_RegisterProgress()
    {
        // 1️⃣ Create item
        var createResponse = await _client.PostAsJsonAsync(
            "/api/Items",
            new
            {
                Id = 1,
                Title = "DDD",
                Description = "End to End",
                Category = "Architecture"
            });

        createResponse.EnsureSuccessStatusCode();

        // 2️⃣ Register progression
        var progressResponse = await _client.PostAsJsonAsync(
            "/api/Items/1/progress",
            new
            {
                Date = DateTime.UtcNow,
                Percent = 50
            });

        progressResponse.EnsureSuccessStatusCode();
    }
}