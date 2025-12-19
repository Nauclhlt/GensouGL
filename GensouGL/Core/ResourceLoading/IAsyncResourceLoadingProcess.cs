namespace GensouGL.Core;

public interface IAsyncResourceLoadingProcess
{
    // Load, OpenGL operations are not allowed in this function.
    public object Resource { get; }
    public Task Load();
    public void Complete();
}