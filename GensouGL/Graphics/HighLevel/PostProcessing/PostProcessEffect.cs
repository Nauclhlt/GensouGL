namespace GensouGL.Graphics;

public abstract class PostProcessEffect
{
    private static QuickRenderer s_sharedQuickRenderer;
    protected static QuickRenderer GetSharedQuickRenderer(Game game, Screen screen)
    {
        if (s_sharedQuickRenderer is null)
        {
            s_sharedQuickRenderer = new(game, screen);
        }
        s_sharedQuickRenderer.SetScreen(screen);
        return s_sharedQuickRenderer;
    }

    protected float _actualSizeScale = 1.0f;

    internal float ActualSizeScale => _actualSizeScale;

    internal abstract void Apply(Game game, Renderer renderer, Screen source, Screen dest);
}