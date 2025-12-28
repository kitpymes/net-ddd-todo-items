using System.Net;
using System.Text.Json.Serialization;
using TodoItems.Domain._Common.AppResults._Settings;

namespace TodoItems.Domain._Common.AppResults;

public class AppResult : IAppResult
{
    [JsonConstructor]
    private AppResult(bool isSuccess) => IsSuccess = isSuccess;

    public bool IsSuccess { get; private set; }

    public int? Status { get; private set; }

    public string? Title { get; private set; }

    public string? TraceId { get; private set; }

    public string? Exception { get; private set; }

    public string? Message { get; private set; }

    public object? Details { get; private set; }

    public object? Data { get; private set; }

    public IDictionary<string, IEnumerable<string>>? Errors { get; private set; }

    public static AppResult Success() => new(true);

    public static AppResult Success(Action<ResultOptionsSuccess> options)
    {
        var settings = new ResultOptionsSuccess();

        options.Invoke(settings);

        var config = settings.ResultSettings;

        return new AppResult(true)
        {
            Status = (int)HttpStatusCode.OK,
            Title = config.Title,
            Data = config.Data,
        };
    }

    public static AppResult BadRequest(string message)
    => Error(x => x
        .WithMessage(message)
        .WithStatusCode(HttpStatusCode.BadRequest)
        .WithTitle("Validation Error"));

    public static AppResult BadRequest(IEnumerable<string> messages)
        => Error(x => x
            .WithMessages(messages)
            .WithStatusCode(HttpStatusCode.BadRequest)
            .WithTitle("Validation Error"));

    public static AppResult BadRequest(IEnumerable<(string fieldName, string message)> errors)
        => Error(x => x
            .WithErrors(errors)
            .WithStatusCode(HttpStatusCode.BadRequest)
            .WithTitle("Validation Error"));

    public static AppResult Error(Action<ResultOptionsError> options)
    {
        var settings = new ResultOptionsError();

        options.Invoke(settings);

        var config = settings.ResultSettings;

        return new AppResult(false)
        {
            Status = config.Status,
            Title = config.Title,
            TraceId = Guid.NewGuid().ToString(),
            Details = config.Details,
            Exception = config.Exception,
            Message = config.Messages?.Any() == true ? string.Join(", ", config.Messages) : null,
            Errors = config.Errors,
        };
    }
}