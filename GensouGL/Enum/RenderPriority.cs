namespace GensouGL.Graphics;

/// <summary>
/// Represents a rendering priority level.
/// </summary>
public enum RenderPriority
{
    /// <summary>
    /// Opaque. Objects with this priority level will be rendered first.
    /// </summary>
    Opaque = 0,
    /// <summary>
    /// Transparent. Objects with this priority level will be rendered after the opaque objects, sorted by the depth in the view space.
    /// </summary>
    Transparent = 1
}