namespace GensouGL.Graphics;

using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;

/// <summary>
/// Manages a array of font atlas pages.
/// </summary>
public sealed class FontAtlasArray : IDisposable
{
    private List<FontAtlasPage> _pages;
    
    private bool _disposed = false;

    /// <summary>
    /// Gets the list of the pages.
    /// </summary>
    public ImmutableList<FontAtlasPage> Pages => _pages.ToImmutableList();

    /// <summary>
    /// Initializes a new instance of <see cref="FontAtlasArray"/>.
    /// </summary>
    public FontAtlasArray()
    {
        _pages = [new()];
    }

    /// <summary>
    /// Gets the atlas texture of the specified page.
    /// </summary>
    /// <param name="page">Page index.</param>
    /// <returns>Atlas texture of the specified page.</returns>
    public ITexture GetPageTexture(int page)
    {
        return _pages[page].Texture;
    }

    /// <summary>
    /// Gets the texture of the specified glyph.
    /// </summary>
    /// <param name="info">Source glyph info.</param>
    /// <returns>Texture of the specified glyph.</returns>
    public ITexture GetGlyphTexture(GlyphInfo info)
    {
        return _pages[info.Page].Texture.GetPartial(info.Coords);
    }

    internal GlyphInfo GenerateGlyph(IMS.Image<IMSP.Rgba32> image)
    {
        if (!_pages[^1].CanPack(image))
        {
            _pages.Add(new());
        }

        Rect coords = _pages[^1].PackUnchecked(image);

        return new GlyphInfo(_pages.Count - 1, coords, image.Width, image.Height);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                for (int i = 0; i < _pages.Count; i++)
                {
                    _pages[i]?.Dispose(); 
                }
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="FontAtlasArray"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="FontAtlasArray"/> destructor.
    /// </summary>
    ~FontAtlasArray()
    {
        Dispose(false);
    }
}