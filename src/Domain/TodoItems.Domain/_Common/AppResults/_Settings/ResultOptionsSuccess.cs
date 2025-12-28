namespace TodoItems.Domain._Common.AppResults._Settings;

public class ResultOptionsSuccess
{
    public ResultSettingsSuccess ResultSettings { get; private set; } = new();

    public virtual ResultOptionsSuccess WithTitle(string title)
    {
        ResultSettings.Title = title;

        return this;
    }

    public virtual ResultOptionsSuccess WithData<T>(T data)
    {
        ResultSettings.Data = data;

        return this;
    }
}
