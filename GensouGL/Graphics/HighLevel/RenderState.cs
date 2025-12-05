namespace GensouGL.Graphics;

public struct RenderState
{
    public static RenderState Default { get; } = new();

    private BlendMode _blendMode = BlendMode.Normal;

    public BlendMode BlendMode
    {
        get => _blendMode;
        set => _blendMode = value;
    }

    public RenderState(BlendMode blendMode)
    {
        _blendMode = blendMode;
    }

    internal void LoadBlendMode()
    {
        GL.BlendEquation(BlendModeData.BlendEquation[(int)_blendMode]);
        GL.BlendFunc(BlendModeData.BlendFactors[(int)_blendMode].src, BlendModeData.BlendFactors[(int)_blendMode].dst);
    }
}