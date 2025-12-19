namespace GensouGL.Core.AsyncLoaders;

public sealed class AsyncTextLoader : IAsyncResourceLoadingProcess
{
    private string _filename;
    private string _text;

    public object Resource => _text;

    public AsyncTextLoader(string filename)
    {
        _filename = filename;
    }

    public async Task Load()
    {
        _text = await File.ReadAllTextAsync(_text);
    }

    public void Complete()
    {
    }
}