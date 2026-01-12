namespace GensouGL.Graphics;

/// <summary>
/// Provides a set of 3D graphics settings.
/// </summary>
public sealed class GraphicsSettings3D
{
    private bool _backfaceCulling = false;

    /// <summary>
    /// Gets or sets whether to cull back-facing polygons.
    /// </summary>
    public bool BackfaceCulling
    {
        get => _backfaceCulling;
        set => _backfaceCulling = value;
    }
}