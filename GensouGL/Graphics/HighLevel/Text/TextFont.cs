namespace GensouGL.Graphics;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IMS = SixLabors.ImageSharp;
using IMSF = SixLabors.Fonts;
using IMSP = SixLabors.ImageSharp.PixelFormats;

// FontAtlasArrayを内部に持ち, グリフ情報を要求されたら, なければ作成, すでにあれば返す.
// グリフを生成する
public sealed class TextFont : IDisposable
{
    private FontAtlasArray _atlasArray;
    private Dictionary<char, GlyphInfo> _glyphs;
    private IMSF.Font _font;
    private FontSettings _settings;

    private bool _disposed = false;

    public bool IsOutlined => _settings.OutlineWidth > 0f;
    public float Size => _font.Size;
    public FontStyleMode Style => _settings.Style;


    public TextFont(FontSource source, float size, FontSettings settings)
    {
        _font = new(source.GetFontFamily(), size, (IMSF.FontStyle)settings.Style);
        _atlasArray = new FontAtlasArray();
        _glyphs = [];
        _settings = settings;
    }

    public TextFont(FontSource source, float size) : this(source, size, FontSettings.Regular) { }
    public TextFont(string filename, float size, FontSettings settings) : this(new FontSource(filename), size, settings) { }
    public TextFont(string filename, float size) : this(filename, size, FontSettings.Regular) { }
    public TextFont(Stream stream, float size, FontSettings settings) : this(new FontSource(stream), size, settings) { }
    public TextFont(Stream stream, float size) : this(stream, size, FontSettings.Regular) { }

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
            w += 2 * (int)_settings.OutlineWidth;
            h += 2 * (int)_settings.OutlineWidth;

            using IMS.Image<IMSP.Rgba32> img = new(w, h, new IMSP.Rgba32(0, 0, 0, 0));

            if (_settings.OutlineWidth <= 0f)
            {
                img.Mutate(x =>
                {
                    x.DrawText(new RichTextOptions(_font)
                    {
                        Origin = new System.Numerics.Vector2((int)_settings.OutlineWidth, (int)_settings.OutlineWidth)
                    }, c.ToString(), IMS.Color.White);
                });
            }
            else
            {
                SolidBrush brush = new(IMS.Color.Black);
                SolidPen pen = new(IMS.Color.White, _settings.OutlineWidth);
                img.Mutate(x =>
                {
                    x.DrawText(new RichTextOptions(_font)
                    {
                    }, c.ToString(), brush, pen);
                });
            }    

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