namespace GensouGL.Graphics;

internal readonly struct Subtexture : ITexture
{
    private readonly int _id;
    private readonly int _baseWidth;
    private readonly int _baseHeight;
    private readonly float _width;
    private readonly float _height;
    private readonly Rect _coords;

    public int TextureId => _id;
    public int BaseWidth => _baseWidth;
    public int BaseHeight => _baseHeight;
    public float Width => _width;
    public float Height => _height;
    public Rect Coords => _coords;

    internal Subtexture(Texture source, Rect coords)
    {
        _id = source.TextureId;
        _baseWidth = source.BaseWidth;
        _baseHeight = source.BaseHeight;
        _width = source.BaseWidth * coords.Width;
        _height = source.BaseHeight * coords.Height;
        _coords = coords;
    }

    internal Subtexture(ITexture source, Rect relativeCoords)
    {
        _id = source.TextureId;
        _baseWidth = source.BaseWidth;
        _baseHeight = source.BaseHeight;
        _width = source.Width * relativeCoords.Width;
        _height = source.Height * relativeCoords.Height;
        _coords = new Rect(source.Coords.X + relativeCoords.X * source.Width / source.BaseWidth,
            source.Coords.Y + relativeCoords.Y * source.Height / source.BaseHeight,
            relativeCoords.Width * source.Width / source.BaseWidth,
            relativeCoords.Height * source.Height / source.BaseHeight);
    }
}