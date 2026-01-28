namespace GensouGL.Graphics;

/// <summary>
/// Represents a render key.
/// </summary>
public readonly struct RenderKey
{
    /// <summary>
    /// Rendering priority level.
    /// </summary>
    public readonly RenderPriority Priority = RenderPriority.Opaque;
    /// <summary>
    /// Representative point.
    /// </summary>
    public readonly Vec3 RepresentativePoint = Vec3.Zero;

    /// <summary>
    /// Initializes <see cref="RenderKey"/>.
    /// </summary>
    public RenderKey()
    {
        Priority = RenderPriority.Opaque;
        RepresentativePoint = Vec3.Zero;
    }

    /// <summary>
    /// Initializes <see cref="RenderKey"/> with the specified render priority and representative point.
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="representativePoint"></param>
    public RenderKey(RenderPriority priority, Vec3 representativePoint)
    {
        Priority = priority;
        RepresentativePoint = representativePoint;
    }
}