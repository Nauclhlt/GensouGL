namespace GensouGL.Graphics;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IMS = SixLabors.ImageSharp;
using IMSF = SixLabors.Fonts;
using IMSP = SixLabors.ImageSharp.PixelFormats;

/// <summary>
/// Represents a text font.
/// </summary>
public sealed class TextFont : IDisposable
{
    private FontAtlasArray _atlasArray;
    private Dictionary<char, GlyphInfo> _glyphs;
    private IMSF.Font _font;
    private FontSettings _settings;

    private bool _disposed = false;

    /// <summary>
    /// Gets if the font is outlined.
    /// </summary>
    public bool IsOutlined => _settings.OutlineWidth > 0f;
    /// <summary>
    /// Gets the font size.
    /// </summary>
    public float Size => _font.Size;
    /// <summary>
    /// Gets the font style.
    /// </summary>
    public FontStyleMode Style => _settings.Style;


    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="source">Font source.</param>
    /// <param name="size">Font size.</param>
    /// <param name="settings">Font settings.</param>
    public TextFont(FontSource source, float size, FontSettings settings)
    {
        _font = new(source.GetFontFamily(), size, (IMSF.FontStyle)settings.Style);
        _atlasArray = new FontAtlasArray();
        _glyphs = [];
        _settings = settings;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="source">Font source.</param>
    /// <param name="size">Font size.</param>
    public TextFont(FontSource source, float size) : this(source, size, FontSettings.Regular) { }
    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="filename">Font file name.</param>
    /// <param name="size">Font size.</param>
    /// <param name="settings">Font settings.</param>
    public TextFont(string filename, float size, FontSettings settings) : this(new FontSource(filename), size, settings) { }
    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="filename">Font file name.</param>
    /// <param name="size">Font size.</param>
    public TextFont(string filename, float size) : this(filename, size, FontSettings.Regular) { }
    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="stream">Source font stream.</param>
    /// <param name="size">Font size.</param>
    /// <param name="settings">Font settings.</param>
    public TextFont(Stream stream, float size, FontSettings settings) : this(new FontSource(stream), size, settings) { }
    /// <summary>
    /// Initializes a new instance of <see cref="TextFont"/> using the specified parameters.
    /// </summary>
    /// <param name="stream">Source font stream.</param>
    /// <param name="size">Font size.</param>
    public TextFont(Stream stream, float size) : this(stream, size, FontSettings.Regular) { }
    
    /// <summary>
    /// Gets the glyph information of the specified character.
    /// </summary>
    /// <param name="c">Target character.</param>
    /// <returns>Glyph information.</returns>
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

    /// <summary>
    /// Gets the height of the specified character.
    /// </summary>
    /// <param name="c">Target character.</param>
    /// <returns>Height of the specified charactter.</returns>
    public int HeightOf(char c)
    {
        return GetGlyph(c).Height;
    }

    /// <summary>
    /// Gets the height of the specified string.
    /// </summary>
    /// <param name="s">Target string.</param>
    /// <returns>Height of the specified string.</returns>
    public int HeightOf(ReadOnlySpan<char> s)
    {
        int max = 0;
        for (int i = 0; i < s.Length; i++)
        {
            max = int.Max(max, HeightOf(s[i]));
        }

        return max;
    }

    /// <summary>
    /// Gets the width of the specified character.
    /// </summary>
    /// <param name="c">Target character.</param>
    /// <returns>Width of the specified charactter.</returns>
    public int WidthOf(char c)
    {
        return GetGlyph(c).Width;
    }

    /// <summary>
    /// Gets the width of the specified string.
    /// </summary>
    /// <param name="s">Target string.</param>
    /// <returns>Width of the specified string.</returns>
    public int WidthOf(ReadOnlySpan<char> s)
    {
        int width = 0;
        for (int i = 0; i < s.Length; i++)
        {
            width += WidthOf(s[i]);
        }

        return width;
    }

    /// <summary>
    /// Gets the atlas texture of the specified page.
    /// </summary>
    /// <param name="page">Page index.</param>
    /// <returns>Atlas texture of the specified page.</returns>
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

    /// <summary>
    /// Releases all resources used by <see cref="TextFont"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="TextFont"/> destructor.
    /// </summary>
    ~TextFont()
    {
        Dispose(false);
    }
}