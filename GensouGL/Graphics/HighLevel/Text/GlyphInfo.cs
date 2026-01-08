using GensouGL.Graphics;

/// <summary>
/// Represents a glyph information.
/// </summary>
public readonly struct GlyphInfo
{
    private readonly int _page;
    private readonly Rect _coords;
    private readonly int _width;
    private readonly int _height;

    /// <summary>
    /// Gets the index of the font atlas page that contains the glyph.
    /// </summary>
    public int Page => _page;
    /// <summary>
    /// Gets the UV coords of the glyph.
    /// </summary>
    public Rect Coords => _coords;
    /// <summary>
    /// Gets the width of the glyph.
    /// </summary>
    public int Width => _width;
    /// <summary>
    /// Gets the height of the glyph.
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// Initializes a <see cref="GlyphInfo"/> using the specified parameters.
    /// </summary>
    /// <param name="page">Page number of the glyph.</param>
    /// <param name="coords">UV coords of the glyph.</param>
    /// <param name="width">Width of the glyph.</param>
    /// <param name="height">Height of the glyph.</param>
    public GlyphInfo(int page, Rect coords, int width, int height)
    {
        _page = page;
        _coords = coords;
        _width = width;
        _height = height;
    }
}