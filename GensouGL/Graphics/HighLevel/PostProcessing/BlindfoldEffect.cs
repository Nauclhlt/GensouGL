namespace GensouGL.Graphics;

public sealed class BlindfoldEffect : PostProcessEffect
{
    private Color _color;
    private Rect _bounds;

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public Rect Bounds
    {
        get => _bounds;
        set => _bounds = value;
    }

    private BlindfoldEffect()
    {
    }

    public BlindfoldEffect(Color color, Rect bounds)
    {
        _color = color;
        _bounds = bounds;
    }

    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        Shader.Blindfold.Bind();
        Shader.Blindfold.SetUniformVector4("pColor", _color.Rf, _color.Gf, _color.Bf, _color.Af);
        
        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}