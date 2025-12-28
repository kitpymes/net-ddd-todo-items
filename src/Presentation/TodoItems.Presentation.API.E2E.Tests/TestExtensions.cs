using Newtonsoft.Json.Linq;
using System.Text.Json;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Presentation.API.E2E.Tests;

public static class TestExtensions
{
    public static async Task<AppResult> ToAppResult(this HttpContent httpContent)
    {
        var content = await httpContent.ReadAsStringAsync();

        dynamic json = JObject.Parse(content);

        var isSuccess = json?["isSuccess"] is not null ? (bool)json?["isSuccess"] : (bool)json?["IsSuccess"];

        if (isSuccess)
        {
            return AppResult.Success();
        }
        else
        {
            var message = json?["message"] is not null ? json?["message"]?.ToString() : json?["Message"]?.ToString();

            return AppResult.BadRequest(message);
        }
    }

    public static async Task<AppResult> ToAppResult<T>(this HttpContent httpContent)
    {
        var content = await httpContent.ReadAsStringAsync();

        dynamic json = JObject.Parse(content);

        var isSuccess = json?["isSuccess"] is not null ? (bool)json?["isSuccess"] : (bool)json?["IsSuccess"];

        if(isSuccess)
        {
            var dataProperty = json?["data"] is not null ? json?["data"] : json?["Data"];

            if (dataProperty != null)
            {
                var data = JsonSerializer.Deserialize<T>(dataProperty.ToString()!, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

                return AppResult.Success(x => x.WithData(data));
            }

            return AppResult.Success();
        } 
        else
        {
            var message = json?["message"] is not null ? json?["message"]?.ToString() : json?["Message"]?.ToString();
            
            return AppResult.BadRequest(message);
        }
    }
}
