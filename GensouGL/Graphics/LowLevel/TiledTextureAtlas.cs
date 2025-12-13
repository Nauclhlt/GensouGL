namespace GensouGL.Graphics;

public sealed class TiledTextureAtlas : IDisposable
{
    private readonly Texture _atlasTexture;
    private readonly int _tileWidth;
    private readonly int _tileHeight;
    private readonly int _countX;
    private readonly int _countY;

    private bool _disposed = false;

    public Texture AtlasTexture => _atlasTexture;
    public int TileWidth => _tileWidth;
    public int TileHeight => _tileHeight;
    public int CountX => _countX;
    public int CountY => _countY;
    public int AtlasWidth => _atlasTexture.BaseWidth;
    public int AtlasHeight => _atlasTexture.BaseHeight;
    public int TileCount => _countX * _countY;

    public TiledTextureAtlas(Texture atlas, int tileWidth, int tileHeight)
    {
        if (atlas.Width % tileWidth != 0)
        {
            throw new ArgumentException("Width of the atlas texture needs to be divisible by tile width.");
        }

        if (atlas.Height % tileHeight != 0)
        {
            throw new ArgumentException("Height of the atlas texture needs to be divisible by tile height.");
        }

        _atlasTexture = atlas;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _countX = _atlasTexture.BaseWidth / tileWidth;
        _countY = _atlasTexture.BaseHeight / tileHeight;
    }

    public ITexture Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _countX || y >= _countY)
        {
            throw new IndexOutOfRangeException();
        }
        
        return _atlasTexture.GetPartial(_tileWidth * x, _tileHeight * y, _tileWidth, _tileHeight);
    }

    public ITexture Get(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new IndexOutOfRangeException();
        }

        return Get(index / _countX, index % _countX);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _atlasTexture?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~TiledTextureAtlas()
    {
        Dispose(false);
    }
}