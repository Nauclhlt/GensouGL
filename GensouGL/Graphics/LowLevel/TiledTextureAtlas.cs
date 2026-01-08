namespace GensouGL.Graphics;

/// <summary>
/// Provides a tiled texture atlas.
/// </summary>
public sealed class TiledTextureAtlas : IDisposable
{
    private readonly Texture _atlasTexture;
    private readonly int _tileWidth;
    private readonly int _tileHeight;
    private readonly int _countX;
    private readonly int _countY;

    private bool _disposed = false;

    /// <summary>
    /// Gets the atlas texture.
    /// </summary>
    public Texture AtlasTexture => _atlasTexture;
    /// <summary>
    /// Gets the width of each tile.
    /// </summary>
    public int TileWidth => _tileWidth;
    /// <summary>
    /// Gets the height of each tile.
    /// </summary>
    public int TileHeight => _tileHeight;
    /// <summary>
    /// Get the number of divisions in the X direction.
    /// </summary>
    public int CountX => _countX;
    /// <summary>
    /// Gets the number of divisions in the Y direction.
    /// </summary>
    public int CountY => _countY;
    /// <summary>
    /// Gets the width of the atlas texture.
    /// </summary>
    public int AtlasWidth => _atlasTexture.BaseWidth;
    /// <summary>
    /// Gets the height of the atlas texture.
    /// </summary>
    public int AtlasHeight => _atlasTexture.BaseHeight;
    /// <summary>
    /// Gets the number of tiles contained.
    /// </summary>
    public int TileCount => _countX * _countY;

    /// <summary>
    /// Initializes a new instance of <see cref="TiledTextureAtlas"/> using the specified atlas texture and size.
    /// </summary>
    /// <param name="atlas">Atlas texture.</param>
    /// <param name="tileWidth">Width of each tile.</param>
    /// <param name="tileHeight">Height of each tile.</param>
    /// <exception cref="ArgumentException">Thrown when the tile size is invalid.</exception>
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

    /// <summary>
    /// Gets the tile at the specified position.
    /// </summary>
    /// <param name="x">X index.</param>
    /// <param name="y">Y index.</param>
    /// <returns>Texture of the specified tile.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is invalid.</exception>
    public ITexture Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _countX || y >= _countY)
        {
            throw new IndexOutOfRangeException();
        }
        
        return _atlasTexture.GetPartial(_tileWidth * x, _tileHeight * y, _tileWidth, _tileHeight);
    }

    /// <summary>
    /// Gets the tile at the specified index.
    /// </summary>
    /// <param name="index">X index.</param>
    /// <returns>Texture of the specified tile.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is invalid.</exception>
    /// <remarks>This function works the same as Get(index / CountX, index % CountX).</remarks>
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

    /// <summary>
    /// Releases all resources used by <see cref="TiledTextureAtlas"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="TiledTextureAtlas"/> destructor.
    /// </summary>
    ~TiledTextureAtlas()
    {
        Dispose(false);
    }
}