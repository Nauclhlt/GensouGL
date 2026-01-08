namespace GensouGL.Graphics;

/// <summary>
/// Provides a base class for all post-processing effects applied to a screen buffer.
/// </summary>
public abstract class PostProcessEffect
{
    private static QuickRenderer2D s_sharedQuickRenderer;

    /// <summary>
    /// Gets or creates a shared <see cref="QuickRenderer2D"/> configured for the specified screen.
    /// </summary>
    /// <param name="screen">The target screen for rendering.</param>
    /// <returns>A shared <see cref="QuickRenderer2D"/> instance.</returns>
    protected static QuickRenderer2D GetSharedQuickRenderer(Screen screen)
    {
        if (s_sharedQuickRenderer is null)
        {
            s_sharedQuickRenderer = new(screen);
        }
        s_sharedQuickRenderer.SetScreen(screen);
        return s_sharedQuickRenderer;
    }

    /// <summary>
    /// Internal scale factor for the effect's processing resolution.
    /// </summary>
    protected float _actualSizeScale = 1.0f;

    internal float ActualSizeScale => _actualSizeScale;

    internal abstract void Apply(Renderer renderer, Screen source, Screen dest);
}