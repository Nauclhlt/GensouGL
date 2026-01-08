namespace GensouGL.Graphics;

/// <summary>
/// A post-processing effect that applies a "blindfold" visual, typically masking areas of the screen with a solid color.
/// </summary>
public sealed class BlindfoldEffect : PostProcessEffect
{
    private Color _color;
    private Rect _bounds;

    /// <summary>
    /// Gets or sets the color of the blindfold mask.
    /// </summary>
    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    /// <summary>
    /// Gets or sets the rectangular boundaries defining the effect area.
    /// </summary>
    public Rect Bounds
    {
        get => _bounds;
        set => _bounds = value;
    }

    private BlindfoldEffect()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlindfoldEffect"/> with default values.
    /// </summary>
    public BlindfoldEffect(Color color, Rect bounds)
    {
        _color = color;
        _bounds = bounds;
    }

    internal override void Apply(Renderer renderer, Screen source, Screen dest)
    {
        Shader.Blindfold.Bind();
        Shader.Blindfold.SetUniformVector4("pColor", _color.R, _color.G, _color.B, _color.A);
        
        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}