namespace GensouGL.Graphics;

using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;


// フォントアトラスのページを管理する.
// 画像を新規登録してグリフ情報を返す.
// グリフ情報からテクスチャを返す.
public sealed class FontAtlasArray : IDisposable
{
    private List<FontAtlasPage> _pages;
    
    private bool _disposed = false;

    public List<FontAtlasPage> Pages => _pages;

    public FontAtlasArray()
    {
        _pages = [new()];
    }

    public ITexture GetPageTexture(int page)
    {
        return _pages[page].Texture;
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FontAtlasArray()
    {
        Dispose(false);
    }
}