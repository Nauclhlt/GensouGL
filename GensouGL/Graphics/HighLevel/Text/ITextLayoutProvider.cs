namespace GensouGL.Graphics;

/// <summary>
/// Represents a base interface for text layout provider classes.
/// </summary>
public interface ITextLayoutProvider
{
    /// <summary>
    /// Determines if the layout provider is ready to begin layouts.
    /// </summary>
    bool CanBegin { get; }
    /// <summary>
    /// Begins a new layout.
    /// </summary>
    /// <param name="font">Font to use.</param>
    /// <param name="layoutSize">Size of the layout.</param>
    void Begin(TextFont font, Vec2 layoutSize);
    /// <summary>
    /// Writes the specified glyph. 
    /// </summary>
    /// <param name="c">Character to write.</param>
    /// <returns>Bounds to place the specified character into.</returns>
    Rect WriteChar(char c);
    /// <summary>
    /// Ends the current layout.
    /// </summary>
    void End();
}