namespace GensouGL.Graphics;

internal sealed class PostProcessScope : IDisposable
{
    private Renderer _renderer;
    private Game _game;
    private PostProcessEffect _effect;

    public PostProcessScope(Renderer renderer, Game game, PostProcessEffect effect)
    {
        _renderer = renderer;
        _game = game;
        _effect = effect;
    }

    public void Dispose()
    {
        Screen source = _renderer.ScreenStack.Pop();

        _effect.Apply(_game, _renderer, source, _renderer.ScreenStack.Peek());

        TempScreenPool.Return(source);
    }
}