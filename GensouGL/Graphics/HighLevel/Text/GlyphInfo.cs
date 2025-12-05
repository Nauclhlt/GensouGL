using GensouGL.Graphics;

public readonly struct GlyphInfo
{
    private readonly int _page;
    private readonly Rect _coords;
    private readonly int _width;
    private readonly int _height;

    public int Page => _page;
    public Rect Coords => _coords;
    public int Width => _width;
    public int Height => _height;

    public GlyphInfo(int page, Rect coords, int width, int height)
    {
        _page = page;
        _coords = coords;
        _width = width;
        _height = height;
    }
}