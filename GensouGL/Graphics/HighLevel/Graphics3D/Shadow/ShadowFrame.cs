namespace GensouGL.Graphics;

/// <summary>
/// Represents a shadow frame data.
/// </summary>
public sealed class ShadowFrame
{
    /// <summary>
    /// Gets the default shadow frame data.
    /// </summary>
    public static ShadowFrame Default => new ShadowFrame();

    private DirectionalShadow _directionalShadow = DirectionalShadow.Default;

    /// <summary>
    /// Gets or sets the directional shadow.
    /// </summary>
    public DirectionalShadow DirectionalShadow
    {
        get => _directionalShadow;
        set => _directionalShadow = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ShadowFrame"/>.
    /// </summary>
    public ShadowFrame()
    {
    }
}