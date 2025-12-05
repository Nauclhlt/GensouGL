namespace GensouGL.Graphics;

using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;
using IMSF = SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

// FontAtlasArrayを内部に持ち, グリフ情報を要求されたら, なければ作成, すでにあれば返す.
// グリフを生成する
public sealed class TextFont : IDisposable
{
    private FontAtlasArray _atlasArray;
    private Dictionary<char, GlyphInfo> _glyphs;
    private IMSF.Font _font;

    private bool _disposed = false;

    public TextFont(FontSource source, float size)
    {
        _font = new(source.GetFontFamily(), size);
        _atlasArray = new FontAtlasArray();
        _glyphs = [];
    }

    public TextFont(string filename, float size) : this(new FontSource(filename), size) { }

    public TextFont(Stream stream, float size) : this(new FontSource(stream), size) { }

    public GlyphInfo GetGlyph(char c)
    {
        if (char.IsWhiteSpace(c)) return GetGlyph('a');
        if (_glyphs.TryGetValue(c, out GlyphInfo glyphInfo))
        {
            return glyphInfo;
        }
        else
        {
            IMSF.TextOptions opt = new (_font);
            IMSF.FontRectangle m = IMSF.TextMeasurer.MeasureBounds(c.ToString(), opt);
            
            int w = (int)float.Ceiling(float.Abs(m.X) + m.Width);
            int h = (int)float.Ceiling(float.Abs(m.Y) + m.Height);
            using IMS.Image<IMSP.Rgba32> img = new(w, h, new IMSP.Rgba32(0, 0, 0, 0));

            img.Mutate(x =>
            {
                x.DrawText(new RichTextOptions(_font)
                {
                    //Origin = new System.Numerics.Vector2(m.X, m.Y)
                }, c.ToString(), IMS.Color.White);
            });

            GlyphInfo info = _atlasArray.GenerateGlyph(img);
            _glyphs.Add(c, info);

            return info;
        }
    }

    public int HeightOf(char c)
    {
        return GetGlyph(c).Height;
    }

    public int HeightOf(ReadOnlySpan<char> s)
    {
        int max = 0;
        for (int i = 0; i < s.Length; i++)
        {
            max = int.Max(max, HeightOf(s[i]));
        }

        return max;
    }

    public int WidthOf(char c)
    {
        return GetGlyph(c).Width;
    }

    public int WidthOf(ReadOnlySpan<char> s)
    {
        int width = 0;
        for (int i = 0; i < s.Length; i++)
        {
            width += WidthOf(s[i]);
        }

        return width;
    }

    public ITexture GetPageTexture(int page)
    {
        return _atlasArray.GetPageTexture(page);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _atlasArray.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~TextFont()
    {
        Dispose(false);
    }
}