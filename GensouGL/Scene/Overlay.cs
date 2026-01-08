namespace GensouGL.Core;

/// <summary>
/// Represents an overlay. Overlays are objects that are drawn on top of the scene, useful for creating dialogue etc.
/// </summary>
public class Overlay : Scene
{
    /// <summary>
    /// Gets the empty overlay.
    /// </summary>
    public new static Overlay Empty { get; } = new Overlay();

    internal bool Closing { get; set; } = false;

    /// <summary>
    /// Closes the current overlay.
    /// </summary>
    protected internal void Close()
    {
        Closing = true;
    }
}