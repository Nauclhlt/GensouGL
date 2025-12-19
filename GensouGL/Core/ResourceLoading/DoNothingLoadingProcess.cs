namespace GensouGL.Core;

public sealed class DoNothingLoadingProcess : IAsyncResourceLoadingProcess
{
    private string _text;
    public object Resource => new();

    public DoNothingLoadingProcess(string text)
    {
        _text = text;
    }

    public async Task Load()
    {
        await Task.Delay(1000);
    }

    public void Complete()
    {
        DebugLog.User("Completed: " + _text);
    }
}