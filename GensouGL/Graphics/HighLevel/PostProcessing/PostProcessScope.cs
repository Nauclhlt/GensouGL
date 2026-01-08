namespace GensouGL.Graphics;

internal sealed class PostProcessScope : IDisposable
{
    private Renderer _renderer;
    private PostProcessEffect _effect;

    public PostProcessScope(Renderer renderer, PostProcessEffect effect)
    {
        _renderer = renderer;
        _effect = effect;
    }

    public void Dispose()
    {
        Screen source = _renderer.ScreenStack.Pop();

        _effect.Apply(_renderer, source, _renderer.ScreenStack.Peek());

        TempScreenPool.Return(source);
    }
}