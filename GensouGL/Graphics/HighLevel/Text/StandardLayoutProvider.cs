namespace GensouGL.Graphics;

/// <summary>
/// Provides standard text layouts.
/// </summary>
public sealed class StandardLayoutProvider : ITextLayoutProvider
{
    /// <summary>
    /// Returns the default instance of <see cref="StandardLayoutProvider"/>.
    /// </summary>
    public static StandardLayoutProvider Default => new();

    private LayoutSettings _settings;

    /// <summary>
    /// Gets the layout settings associated with this text layout provider.
    /// </summary>
    public LayoutSettings Settings => _settings;

    /// <summary>
    /// Initializes a new instance of <see cref="StandardLayoutProvider"/>.
    /// </summary>
    /// <param name="settings">Layout settings to be used.</param>
    public StandardLayoutProvider(LayoutSettings settings = null)
    {
        _settings = settings;
        _settings ??= LayoutSettings.Default;
    }

    /// <summary>
    /// Determines if the current layout provider is ready to begin layouts.
    /// </summary>
    public bool CanBegin => _font is null;

    
    // context info
    private TextFont _font;
    private Vec2 _size;
    private Vec2 _position;
    private float _maxHeight;
    //


    /// <summary>
    /// Begins layout.
    /// </summary>
    /// <param name="font">Font to use.</param>
    /// <param name="layoutSize">Layout size.</param>
    public void Begin(TextFont font, Vec2 layoutSize)
    {
        if (!CanBegin) return;
        _font = font;
        _size = layoutSize;
        _position = new Vec2(_settings.Padding);
        _maxHeight = 0f;
    }

    /// <summary>
    /// Writes the specified glyph. 
    /// </summary>
    /// <param name="c">Character to write.</param>
    /// <returns>Bounds to place the specified character into.</returns>
    public Rect WriteChar(char c)
    {
        if (_font is null) return Rect.Zero;

        if (c == '\r')
        {
            return Rect.Zero;
        }

        float glyphWidth, glyphHeight;
        bool visible = false;
        if (c == ' ')
        {
            if (_settings.IgnoreSpace) return Rect.Zero;

            GlyphInfo ghost = _font.GetGlyph(_settings.SpaceGhost);
            glyphWidth = ghost.Width;
            glyphHeight = ghost.Height;
        }
        else if (c == '\t')
        {
            GlyphInfo ghost = _font.GetGlyph(_settings.SpaceGhost);
            glyphWidth = ghost.Width * _settings.TabWidth;
            glyphHeight = ghost.Height;
        }
        else if (c == '\n')
        {
            _position.X = _settings.Padding;
            _position.Y += _maxHeight + _settings.LineMargin;
            _maxHeight = 0f;

            return Rect.Zero;
        }
        else
        {
            GlyphInfo glyph = _font.GetGlyph(c);
            glyphWidth = glyph.Width;
            glyphHeight = glyph.Height;
            visible = true;
        }

        // breaking line if needed
        if (_settings.BreakLines && _position.X + glyphWidth > _size.X - _settings.Padding)
        {
            _position.X = _settings.Padding;
            _position.Y += _maxHeight + _settings.LineMargin;
            _maxHeight = 0f;
        }

        // place
        Rect bounds = new(_position.X, _position.Y, glyphWidth, glyphHeight);

        _position.X += glyphWidth;
        _maxHeight = float.Max(_maxHeight, glyphHeight);

        if (!visible)
        {
            return Rect.Zero;
        }

        return bounds;
    }

    /// <summary>
    /// Ends the current layout.
    /// </summary>
    public void End()
    {
        _font = null;
        _size = Vec2.Zero;
    }

    /// <summary>
    /// Provides text layout settings.
    /// </summary>
    public sealed class LayoutSettings
    {
        private bool _breakLines = true;
        private int _tabWidth = 4;
        private bool _ignoreSpace = false;
        private float _lineMargin = 2f;
        private float _padding = 0f;
        private char _spaceGhost = 'A';

        /// <summary>
        /// Returns the default instance of <see cref="LayoutSettings"/>.
        /// </summary>
        public static LayoutSettings Default => new LayoutSettings();

        /// <summary>
        /// Gets or sets whether to break lines.
        /// </summary>
        public bool BreakLines
        {
            get => _breakLines;
            set => _breakLines = value;
        }

        /// <summary>
        /// Gets or sets the width of a tab character in characters unit.
        /// </summary>
        public int TabWidth
        {
            get => _tabWidth;
            set => _tabWidth = int.Max(0, value);
        }

        /// <summary>
        /// Gets or sets whether to ignore space characters. If true, 'hello world' will look like 'helloworld'
        /// </summary>
        public bool IgnoreSpace
        {
            get => _ignoreSpace;
            set => _ignoreSpace = value;
        }

        /// <summary>
        /// Gets or sets the margin between lines.
        /// </summary>
        public float LineMargin
        {
            get => _lineMargin;
            set => _lineMargin = value;
        }

        /// <summary>
        /// Gets or sets the padding around the text body.
        /// </summary>
        public float Padding
        {
            get => _padding;
            set => _padding = value;
        }

        /// <summary>
        /// Gets or sets the ghost for space characters.
        /// </summary>
        public char SpaceGhost
        {
            get => _spaceGhost;
            set => _spaceGhost = value;
        }
    }
}