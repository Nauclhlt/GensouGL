namespace GensouGL.Graphics;

public readonly struct Subtexture : ITexture
{
    private readonly int _id;
    private readonly int _width;
    private readonly int _height;
    private readonly Rect _coords;

    public int TextureId => _id;
    public int Width => _width;
    public int Height => _height;
    public Rect Coords => _coords;

    internal Subtexture(Texture source, Rect coords)
    {
        _id = source.TextureId;
        _width = source.Width;
        _height = source.Height;
        _coords = coords;
    }
}