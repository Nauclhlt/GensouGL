namespace GensouGL.Core.AsyncLoaders;

internal sealed class AsyncStaticModelLoader : IAsyncResourceLoadingProcess
{
    private string _source;
    private StaticModel3D _model;

    public object Resource => _model;

    public AsyncStaticModelLoader(string source)
    {
        _source = source;
    }

    public async Task Load()
    {
    }

    public void Complete()
    {
        _model = StaticModel3D.Load(_source);
    }
}