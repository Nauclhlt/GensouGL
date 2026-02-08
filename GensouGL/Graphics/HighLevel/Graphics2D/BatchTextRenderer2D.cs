namespace GensouGL.Graphics;

/// <summary>
/// Represents a high-performance text renderer that batches draw calls by font pages to minimize GPU state changes.
/// Supports basic text, gradients, alignment, and outlined rendering.
/// </summary>
public sealed class BatchTextRenderer2D : Renderer2D
{
    private static BatchTextRendererBuffer _buffer;

    private BatchTextRenderer2D() { }

    /// <summary>
    /// Initializes a new instance of <see cref="BatchTextRenderer2D"/> with the specified screen.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public BatchTextRenderer2D(Screen screen)
    {
        Setup(screen);
        _buffers = [new()];
        _used = [false];
        _layoutProvider = StandardLayoutProvider.Default;
    }

    /// <summary>
    /// Initializes static resources required for text rendering.
    /// </summary>
    internal static void Initialize() => _buffer = new();

    /// <summary>
    /// Releases static resources used by the renderer.
    /// </summary>
    internal static void Release() => _buffer.Dispose();

    private TextFont _currentFont = null;
    private List<PageBuffer> _buffers;
    private List<bool> _used;
    private ITextLayoutProvider _layoutProvider;

    /// <summary>
    /// Prepares the renderer for drawing text using the specified font.
    /// Must be called before any Draw calls.
    /// </summary>
    /// <param name="font">The font to use for subsequent draw operations.</param>
    public void Begin(TextFont font)
    {
        if (_currentFont is not null)
        {
            DebugLog.Warn($"{nameof(BatchTextRenderer2D)}: Begin() called before End().");
        }

        _currentFont = font;
        for (int i = 0; i < _buffers.Count; i++)
        {
            _buffers[i].Clear();
            _used[i] = false;
        }
    }

    /// <summary>
    /// Finalizes the batch and submits all accumulated text data to the GPU.
    /// </summary>
    public void End()
    {
        if (_currentFont is null)
        {
            DebugLog.Error($"{nameof(BatchTextRenderer2D)}: End() called before Begin().");
            return;
        }

        LoadState();

        // Select shader based on font properties
        if (_currentFont.IsOutlined)
            Shader.OutlinedText.Bind();
        else
            Shader.SimpleText.Bind();

        Shader.SendMatrix0(GetDefaultVMatrix());

        for (int p = 0; p < _buffers.Count; p++)
        {
            if (!_used[p]) continue;

            TextureService.PushAndBind(_currentFont.GetPageTexture(p).TextureId);

            if (_currentFont.IsOutlined)
            {
                _buffer.Draw(PrimitiveType.Triangles, _buffers[p].Vertices, _buffers[p].Colors, _buffers[p].TexCoords, _buffers[p].OutlineColors);
            }
            else
            {
                _buffer.Draw(PrimitiveType.Triangles, _buffers[p].Vertices, _buffers[p].Colors, _buffers[p].TexCoords);
            }

            TextureService.Pop();
        }

        _currentFont = null;
        RestoreState();
    }

    /// <summary>
    /// Sets the current text layout provider.
    /// </summary>
    /// <param name="provider">New text layout provider.</param>
    public void SetLayoutProvider(ITextLayoutProvider provider)
    {
        if (provider is null) return;
        _layoutProvider = provider;
    }

    #region Centered Drawing Methods

    /// <summary>
    /// Draws text centered within a specified rectangle.
    /// </summary>
    public void DrawCentered(Rect rect, string text, Color color) => DrawCentered(rect.X, rect.Y, rect.Width, rect.Height, text, color);

    /// <summary>
    /// Draws text centered within the specified bounds.
    /// </summary>
    public void DrawCentered(float x, float y, float width, float height, string text, Color color)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);
        Draw(x + (width - tw) / 2, y + (height - th) / 2, text, color);
    }

    /// <summary>
    /// Draws text with a color gradient centered within a rectangle.
    /// </summary>
    public void DrawGradientCentered(Rect rect, string text, RectGradient gradient) => DrawGradientCentered(rect.X, rect.Y, rect.Width, rect.Height, text, gradient);

    /// <summary>
    /// Draws text with a color gradient centered within the specified bounds.
    /// </summary>
    public void DrawGradientCentered(float x, float y, float width, float height, string text, RectGradient gradient)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);
        DrawGradient(x + (width - tw) / 2, y + (height - th) / 2, text, gradient);
    }

    #endregion

    #region Standard Drawing Methods

    /// <summary>
    /// Draws text at the specified point.
    /// </summary>
    public void Draw(Point point, string text, Color color) => Draw(point.X, point.Y, text, color);

    /// <summary>
    /// Draws text at the specified coordinates.
    /// </summary>
    public void Draw(float x, float y, string text, Color color) => DrawGradient(x, y, text, new RectGradient(color));

    /// <summary>
    /// Draws text with a gradient at the specified point.
    /// </summary>
    public void DrawGradient(Point point, string text, RectGradient gradient) => DrawGradient(point.X, point.Y, text, gradient);

    /// <summary>
    /// Accumulates text into the batch buffers, handling layout, spacing, and special characters.
    /// </summary>
    public void DrawGradient(float x, float y, string text, RectGradient gradient)
        => DrawGradient(x, y, 1000000000f, 1000000000f, text, gradient);

    #endregion

    #region Rect Drawing Methods

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    public void Draw(Rect rect, string text, Color color)
        => DrawGradient(rect.X, rect.Y, rect.Width, rect.Height, text, new RectGradient(color));

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    /// <param name="x">X of the rectangle.</param>
    /// <param name="y">Y of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="text">Text.</param>
    /// <param name="color">Color of the text.</param>
    public void Draw(float x, float y, float width, float height, string text, Color color)
        => DrawGradient(x, y, width, height, text, new(color));

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    public void DrawGradient(Rect rect, string text, RectGradient gradient)
        => DrawGradient(rect.X, rect.Y, rect.Width, rect.Height, text, gradient);

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    /// <param name="x">X of the rectangle.</param>
    /// <param name="y">Y of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="text">Text.</param>
    /// <param name="gradient">Gradient colors.</param>
    public void DrawGradient(float x, float y, float width, float height, string text, RectGradient gradient)
    {
        if (_currentFont is null)
        {
            DebugLog.Error($"{nameof(BatchTextRenderer2D)}: Drawing function called before Begin().");
            return;
        }

        if (!_layoutProvider.CanBegin) return;

        _layoutProvider.Begin(_currentFont, new Vec2(width, height));

        ReadOnlySpan<char> span = text.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            Rect bounds = _layoutProvider.WriteChar(span[i]);
            if (bounds.Width == 0f) continue;

            GlyphInfo info = _currentFont.GetGlyph(span[i]);

            while (info.Page >= _buffers.Count)
            {
                _buffers.Add(new());
                _used.Add(false);
            }

            if (_currentFont.IsOutlined)
                _buffers[info.Page].AddWithBorder(x + bounds.X, y + bounds.Y, bounds.Width, bounds.Height, info.Coords, gradient, Color.Black);
            else
                _buffers[info.Page].Add(x + bounds.X, y + bounds.Y, bounds.Width, bounds.Height, info.Coords, gradient);

            _used[info.Page] = true;
        }

        _layoutProvider.End();
    }

    #endregion

    #region Outlined Drawing Methods

    /// <summary>
    /// Draws text with an outline at the specified point.
    /// </summary>
    public void DrawOutlined(Point point, string text, Color color, Color outlineColor) => DrawOutlined(point.X, point.Y, text, color, outlineColor);

    /// <summary>
    /// Draws text with an outline at the specified coordinates.
    /// </summary>
    public void DrawOutlined(float x, float y, string text, Color color, Color outlineColor) => DrawGradientOutlined(x, y, text, new RectGradient(color), outlineColor);

    /// <summary>
    /// Draws outlined text centered within a rectangle.
    /// </summary>
    public void DrawOutlinedCentered(Rect rect, string text, Color color, Color outlineColor) => DrawOutlinedCentered(rect.X, rect.Y, rect.Width, rect.Height, text, color, outlineColor);

    /// <summary>
    /// Draws outlined text centered within specified bounds.
    /// </summary>
    public void DrawOutlinedCentered(float x, float y, float width, float height, string text, Color color, Color outlineColor) => DrawGradientOutlinedCentered(x, y, width, height, text, new RectGradient(color), outlineColor);

    /// <summary>
    /// Draws outlined text with a gradient centered within a rectangle.
    /// </summary>
    public void DrawGradientOutlinedCentered(Rect rect, string text, RectGradient gradient, Color outlineColor) => DrawGradientOutlinedCentered(rect.X, rect.Y, rect.Width, rect.Height, text, gradient, outlineColor);

    /// <summary>
    /// Draws outlined text with a gradient centered within specified bounds.
    /// </summary>
    public void DrawGradientOutlinedCentered(float x, float y, float width, float height, string text, RectGradient gradient, Color outlineColor)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);
        DrawGradientOutlined(x + (width - tw) / 2, y + (height - th) / 2, text, gradient, outlineColor);
    }

    /// <summary>
    /// Draws outlined text with a gradient at the specified point.
    /// </summary>
    public void DrawGradientOutlined(Point point, string text, RectGradient gradient, Color outlineColor) => DrawGradientOutlined(point.X, point.Y, text, gradient, outlineColor);

    /// <summary>
    /// Accumulates outlined text into batch buffers. Requires a font initialized with outline support.
    /// </summary>
    public void DrawGradientOutlined(float x, float y, string text, RectGradient gradient, Color outlineColor)
        => DrawGradientOutlined(x, y, 1000000000f, 1000000000f, text, gradient, outlineColor);

    #endregion

    #region Outlined Rect Drawing Methods

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    public void DrawOutlined(Rect rect, string text, Color color, Color outlineColor)
        => DrawGradientOutlined(rect.X, rect.Y, rect.Width, rect.Height, text, new RectGradient(color), outlineColor);

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    /// <param name="x">X of the rectangle.</param>
    /// <param name="y">Y of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="text">Text.</param>
    /// <param name="color">Color of the text.</param>
    public void DrawOutlined(float x, float y, float width, float height, string text, Color color, Color outlineColor)
        => DrawGradientOutlined(x, y, width, height, text, new(color), outlineColor);

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    public void DrawGradientOutlined(Rect rect, string text, RectGradient gradient, Color outlineColor)
        => DrawGradientOutlined(rect.X, rect.Y, rect.Width, rect.Height, text, gradient, outlineColor);

    /// <summary>
    /// Draws the text to fit within the specified rectangle.
    /// </summary>
    /// <param name="x">X of the rectangle.</param>
    /// <param name="y">Y of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="text">Text.</param>
    /// <param name="gradient">Gradient colors.</param>
    /// <param name="outlineColor">Outline color.</param>
    public void DrawGradientOutlined(float x, float y, float width, float height, string text, RectGradient gradient, Color outlineColor)
    {
        if (_currentFont is null)
        {
            DebugLog.Error($"{nameof(BatchTextRenderer2D)}: Drawing function called before Begin().");
            return;
        }

        if (!_currentFont.IsOutlined)
        {
            DebugLog.Error($"{nameof(BatchTextRenderer2D)}: DrawGradientOutlined() called while outline is disabled.");
            return;
        }

        if (!_layoutProvider.CanBegin) return;

        _layoutProvider.Begin(_currentFont, new Vec2(width, height));

        ReadOnlySpan<char> span = text.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            Rect bounds = _layoutProvider.WriteChar(span[i]);
            if (bounds.Width == 0f) continue;

            GlyphInfo info = _currentFont.GetGlyph(span[i]);

            while (info.Page >= _buffers.Count)
            {
                _buffers.Add(new());
                _used.Add(false);
            }

            _buffers[info.Page].AddWithBorder(x + bounds.X, y + bounds.Y, bounds.Width, bounds.Height, info.Coords, gradient, outlineColor);

            _used[info.Page] = true;
        }

        _layoutProvider.End();
    }

    #endregion
}

internal sealed class BatchTextRendererBuffer: IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _tbuf;
    private int _ocbuf;

    private bool _disposed = false;

    public BatchTextRendererBuffer()
    {
        _vao = GL.GenVertexArray();
        _vbuf = GL.GenBuffer();
        _cbuf = GL.GenBuffer();
        _tbuf = GL.GenBuffer();
        _ocbuf = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
        GL.BindVertexArray(0);
    }

    private void Buffer(int buffer, List<float> a)
    {
        Span<float> span = CollectionsMarshal.AsSpan(a);

        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * span.Length, ref span[0], BufferUsageHint.DynamicDraw);
    }

    public void Draw(PrimitiveType type, List<float> va, List<float> ca, List<float> ta, List<float> oca)
    {
        Buffer(_vbuf, va);
        Buffer(_cbuf, ca);
        Buffer(_tbuf, ta);
        Buffer(_ocbuf, oca);

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _ocbuf);

        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.DrawArrays(type, 0, va.Count / 2);

        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, List<float> va, List<float> ca, List<float> ta)
    {
        Buffer(_vbuf, va);
        Buffer(_cbuf, ca);
        Buffer(_tbuf, ta);
        
        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.DrawArrays(type, 0, va.Count / 2);

        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbuf);
            GL.DeleteBuffer(_cbuf);
            GL.DeleteBuffer(_tbuf);
            GL.DeleteBuffer(_ocbuf);

            _disposed = true;
        }
    }
}

internal sealed class PageBuffer
{
    private List<float> _vertices;
    private List<float> _colors;
    private List<float> _texCoords;
    private List<float> _outlineColors;

    public List<float> Vertices => _vertices;
    public List<float> Colors => _colors;
    public List<float> TexCoords => _texCoords;
    public List<float> OutlineColors => _outlineColors;

    public PageBuffer()
    {
        _vertices = new (256);
        _colors = new (256);
        _texCoords = new (256);
        _outlineColors = new (256);
    }

    public void AddWithBorder(float x, float y, float w, float h, Rect uv, RectGradient gradient, Color outlineColor)
    {
        Add(x, y, w, h, uv, gradient);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);

        _outlineColors.Add(outlineColor.R);
        _outlineColors.Add(outlineColor.G);
        _outlineColors.Add(outlineColor.B);
        _outlineColors.Add(outlineColor.A);
    }

    public void Add(float x, float y, float w, float h, Rect uv, RectGradient gradient)
    {
        _vertices.Add(x);
        _vertices.Add(y);

        _vertices.Add(x);
        _vertices.Add(y + h);

        _vertices.Add(x + w);
        _vertices.Add(y);

        _vertices.Add(x + w);
        _vertices.Add(y);

        _vertices.Add(x);
        _vertices.Add(y + h);

        _vertices.Add(x + w);
        _vertices.Add(y + h);

        _colors.Add(gradient.TopLeft.R);
        _colors.Add(gradient.TopLeft.G);
        _colors.Add(gradient.TopLeft.B);
        _colors.Add(gradient.TopLeft.A);

        _colors.Add(gradient.BottomLeft.R);
        _colors.Add(gradient.BottomLeft.G);
        _colors.Add(gradient.BottomLeft.B);
        _colors.Add(gradient.BottomLeft.A);

        _colors.Add(gradient.TopRight.R);
        _colors.Add(gradient.TopRight.G);
        _colors.Add(gradient.TopRight.B);
        _colors.Add(gradient.TopRight.A);

        _colors.Add(gradient.TopRight.R);
        _colors.Add(gradient.TopRight.G);
        _colors.Add(gradient.TopRight.B);
        _colors.Add(gradient.TopRight.A);

        _colors.Add(gradient.BottomLeft.R);
        _colors.Add(gradient.BottomLeft.G);
        _colors.Add(gradient.BottomLeft.B);
        _colors.Add(gradient.BottomLeft.A);

        _colors.Add(gradient.BottomRight.R);
        _colors.Add(gradient.BottomRight.G);
        _colors.Add(gradient.BottomRight.B);
        _colors.Add(gradient.BottomRight.A);

        _texCoords.Add(uv.X);
        _texCoords.Add(uv.Y);

        _texCoords.Add(uv.X);
        _texCoords.Add(uv.Bottom);

        _texCoords.Add(uv.Right);
        _texCoords.Add(uv.Y);

        _texCoords.Add(uv.Right);
        _texCoords.Add(uv.Y);

        _texCoords.Add(uv.X);
        _texCoords.Add(uv.Bottom);

        _texCoords.Add(uv.Right);
        _texCoords.Add(uv.Bottom);
    }

    public void Clear()
    {
        _vertices.Clear();
        _colors.Clear();
        _texCoords.Clear();
        _outlineColors.Clear();
    }
}