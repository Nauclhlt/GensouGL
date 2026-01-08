namespace GensouGL.Graphics;

/// <summary>
/// Represents a base interface for screens.
/// </summary>
public interface IScreen
{
    /// <summary>
    /// Gets the screen id.
    /// </summary>
    int ScreenId { get; }
    /// <summary>
    /// Gets the screen width.
    /// </summary>
    int Width { get; }
    /// <summary>
    /// Gets the screen height.
    /// </summary>
    int Height { get; }
}