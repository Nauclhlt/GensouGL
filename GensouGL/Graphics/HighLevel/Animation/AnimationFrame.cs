namespace GensouGL.Graphics;

/// <summary>
/// Represents a animation frame.
/// </summary>
/// <param name="duration">Duration of the frame.</param>
/// <param name="source">Source rectangle.</param>
public struct AnimationFrame(float duration, Rect source)
{
    /// <summary>
    /// Duration of the frame.
    /// </summary>
    public float Duration = duration;
    /// <summary>
    /// Source rectangle.
    /// </summary>
    public Rect Source = source;
}