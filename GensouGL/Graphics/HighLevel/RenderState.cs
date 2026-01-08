namespace GensouGL.Graphics;

/// <summary>
/// Represents a rendering state.
/// </summary>
public struct RenderState
{
    /// <summary>
    /// Gets the default rendering state.
    /// </summary>
    public static RenderState Default { get; } = new();

    private BlendMode _blendMode = BlendMode.Normal;

    /// <summary>
    /// Gets or sets the blend mode.
    /// </summary>
    public BlendMode BlendMode
    {
        get => _blendMode;
        set => _blendMode = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="RenderState"/> with the specified blend mode.
    /// </summary>
    /// <param name="blendMode">Blend mode.</param>
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