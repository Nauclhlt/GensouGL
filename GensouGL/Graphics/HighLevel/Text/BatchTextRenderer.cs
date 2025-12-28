namespace GensouGL.Graphics;

public sealed class BatchTextRenderer : Renderer
{
    private static BatchTextRendererBuffer _buffer;

    private BatchTextRenderer()
    {
    }

    internal BatchTextRenderer(Game game, Screen screen)
    {
        Setup(game, screen);
        _buffers = [new()];
        _used = [false];
    }

    internal static void Initialize()
    {
        _buffer = new();
    }

    internal static void Release()
    {
        _buffer.Dispose();
    }



    private TextFont _currentFont = null;
    private List<PageBuffer> _buffers;
    private List<bool> _used;

    public void Begin(TextFont font)
    {
        if (_currentFont is not null)
        {
#if DEBUG
            DebugLog.Warn("BatchTextRenderer: Begin() called before End().");
#endif
        }

        _currentFont = font;
        for (int i = 0; i <  _buffers.Count; i++)
        {
            _buffers[i].Clear();
            _used[i] = false;
        }
    }

    public void End()
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer: End() called before Begin().");
#endif
            return;
        }

        LoadState();

        if (_currentFont.IsOutlined)
        {
            Shader.OutlinedText.Bind();
        }
        else
        {
            Shader.SimpleText.Bind();
        }
            
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

    public void DrawCentered(Rect rect, string text, Color color)
    {
        DrawCentered(rect.X, rect.Y, rect.Width, rect.Height, text, color);
    }

    public void DrawCentered(float x, float y, float width, float height, string text, Color color)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);

        Draw(x + (width - tw) / 2, y + (height - th) / 2, text, color);
    }

    public void DrawGradientCentered(Rect rect, string text, RectGradient gradient)
    {
        DrawGradientCentered(rect.X, rect.Y, rect.Width, rect.Height, text, gradient);
    }

    public void DrawGradientCentered(float x, float y, float width, float height, string text, RectGradient gradient)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);

        DrawGradient(x + (width - tw) / 2, y + (height - th) / 2, text, gradient);
    }

    public void Draw(Point point, string text, Color color)
    {
        Draw(point.X, point.Y, text, color);
    }

    public void Draw(float x, float y, string text, Color color)
    {
        DrawGradient(x, y, text, new RectGradient(color));
    }

    public void DrawGradient(Point point, string text, RectGradient gradient)
    {
        DrawGradient(point.X, point.Y, text, gradient);
    }

    public void DrawGradient(float x, float y, string text, RectGradient gradient)
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer: Drawing function called before Begin().");
#endif
            return;
        }

        float cursorX = x;
        float cursorY = y;
        int lineHeight = 0;

        ReadOnlySpan<char> span = text.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\r') continue;
            if (span[i] == '\n')
            {
                cursorX = x;
                cursorY += lineHeight;
                lineHeight = 0;
                continue;
            }
            if (span[i] == ' ')
            {
                cursorX += _currentFont.WidthOf('w');
                lineHeight = int.Max(lineHeight, _currentFont.HeightOf('w'));
                continue;
            }
            if (span[i] == '\t')
            {
                cursorX += _currentFont.WidthOf('w') * 4;
                lineHeight = int.Max(lineHeight, _currentFont.HeightOf('w'));
                continue;
            }

            GlyphInfo info = _currentFont.GetGlyph(span[i]);

            while (info.Page >= _buffers.Count)
            {
                _buffers.Add(new());
                _used.Add(false);
            }

            if (_currentFont.IsOutlined)
                _buffers[info.Page].AddWithBorder(cursorX, cursorY, info.Width, info.Height, info.Coords, gradient, Color.Black);
            else
                _buffers[info.Page].Add(cursorX, cursorY, info.Width, info.Height, info.Coords, gradient);
            _used[info.Page] = true;

            cursorX += info.Width;
            lineHeight = int.Max(lineHeight, _currentFont.HeightOf(span[i]));
        }
    }






    public void DrawOutlined(Point point, string text, Color color, Color outlineColor)
    {
        DrawOutlined(point.X, point.Y, text, color, outlineColor);
    }

    public void DrawOutlined(float x, float y, string text, Color color, Color outlineColor)
    {
        DrawGradientOutlined(x, y, text, new RectGradient(color), outlineColor);
    }

    public void DrawOutlinedCentered(Rect rect, string text, Color color, Color outlineColor)
    {
        DrawOutlinedCentered(rect.X, rect.Y, rect.Width, rect.Height, text, color, outlineColor);
    }

    public void DrawOutlinedCentered(float x, float y, float width, float height, string text, Color color, Color outlineColor)
    {
        DrawGradientOutlinedCentered(x, y, width, height, text, new RectGradient(color), outlineColor);
    }

    public void DrawGradientOutlinedCentered(Rect rect, string text, RectGradient gradient, Color outlineColor)
    {
        DrawGradientOutlinedCentered(rect.X, rect.Y, rect.Width, rect.Height, text, gradient, outlineColor);
    }

    public void DrawGradientOutlinedCentered(float x, float y, float width, float height, string text, RectGradient gradient, Color outlineColor)
    {
        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);
        DrawGradientOutlined(x + (width - tw) / 2, y + (height - th) / 2, text, gradient, outlineColor);
    }

    public void DrawGradientOutlined(Point point, string text, RectGradient gradient, Color outlineColor)
    {
        DrawGradientOutlined(point.X, point.Y, text, gradient, outlineColor);
    }

    public void DrawGradientOutlined(float x, float y, string text, RectGradient gradient, Color outlineColor)
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer: Drawing function called before Begin().");
#endif
            return;
        }

        if (!_currentFont.IsOutlined)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer: DrawGradientOutlined() called while outline is disabled.");
#endif
            return;
        }

        float cursorX = x;
        float cursorY = y;
        int lineHeight = 0;

        ReadOnlySpan<char> span = text.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\r') continue;
            if (span[i] == '\n')
            {
                cursorX = x;
                cursorY += lineHeight;
                lineHeight = 0;
                continue;
            }
            if (span[i] == ' ')
            {
                cursorX += _currentFont.WidthOf('w');
                lineHeight = int.Max(lineHeight, _currentFont.HeightOf('w'));
                continue;
            }
            if (span[i] == '\t')
            {
                cursorX += _currentFont.WidthOf('w') * 4;
                lineHeight = int.Max(lineHeight, _currentFont.HeightOf('w'));
                continue;
            }

            GlyphInfo info = _currentFont.GetGlyph(span[i]);

            while (info.Page >= _buffers.Count)
            {
                _buffers.Add(new());
                _used.Add(false);
            }

            _buffers[info.Page].AddWithBorder(cursorX, cursorY, info.Width, info.Height, info.Coords, gradient, outlineColor);
            _used[info.Page] = true;

            cursorX += info.Width;
            lineHeight = int.Max(lineHeight, _currentFont.HeightOf(span[i]));
        }
    }
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
        _vertices = new List<float>();
        _colors = new List<float>();
        _texCoords = new List<float>();
        _outlineColors = new List<float>();
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