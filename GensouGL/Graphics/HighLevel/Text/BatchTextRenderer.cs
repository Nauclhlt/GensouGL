using System.Reflection.Metadata.Ecma335;

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
            DebugLog.Error("BatchTextRenderer : Begin() called before End().");
#endif
            return;
        }

        _currentFont = font;
        for (int i = 0; i <  _buffers.Count; i++)
        {
            _buffers[i].Clear();
        }
    }

    public void DrawCentered(Rect rect, string text, Color color)
    {
        DrawCentered(rect.X, rect.Y, rect.Width, rect.Height, text, color);
    }

    public void DrawCentered(float x, float y, float width, float height, string text, Color color)
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer : Draw() called before Begin().");
#endif
            return;
        }

        float tw = _currentFont.WidthOf(text);
        float th = _currentFont.HeightOf(text);

        Draw(x + (width - tw) / 2, y + (height - th) / 2, text, color);
    }

    public void Draw(Point point, string text, Color color)
    {
        Draw(point.X, point.Y, text, color);
    }

    public void Draw(float x, float y, string text, Color color)
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer : Draw() called before Begin().");
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

            _buffers[info.Page].Add(cursorX, cursorY, info.Width, info.Height, info.Coords, color);
            _used[info.Page] = true;

            cursorX += info.Width;
            lineHeight = int.Max(lineHeight, _currentFont.HeightOf(span[i]));
        }
    }

    public void End()
    {
        if (_currentFont is null)
        {
#if DEBUG
            DebugLog.Error("BatchTextRenderer : End() called before Begin().");
#endif
            return;
        }

        LoadState();

        Shader.SimpleTexture.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        for (int p = 0; p < _buffers.Count; p++)
        {
            if (!_used[p]) continue;

            TextureService.PushAndBind(_currentFont.GetPageTexture(p).TextureId);

            _buffer.Draw(PrimitiveType.Triangles, _buffers[p].Vertices, _buffers[p].Colors, _buffers[p].TexCoords);

            TextureService.Pop();
        }

        _currentFont = null;
        RestoreState();
    }
}

internal sealed class BatchTextRendererBuffer: IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _tbuf;

    private bool _disposed = false;

    public BatchTextRendererBuffer()
    {
        _vao = GL.GenVertexArray();
        _vbuf = GL.GenBuffer();
        _cbuf = GL.GenBuffer();
        _tbuf = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, List<float> va, List<float> ca, List<float> ta)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        Span<float> vspan = CollectionsMarshal.AsSpan(va);
        Span<float> cspan = CollectionsMarshal.AsSpan(ca);
        Span<float> tspan = CollectionsMarshal.AsSpan(ta);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Count, ref vspan[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ca.Count, ref cspan[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ta.Count, ref tspan[0], BufferUsageHint.DynamicDraw);


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

            _disposed = true;
        }
    }
}

internal sealed class PageBuffer
{
    private List<float> _vertices;
    private List<float> _colors;
    private List<float> _texCoords;

    public List<float> Vertices => _vertices;
    public List<float> Colors => _colors;
    public List<float> TexCoords => _texCoords;

    public PageBuffer()
    {
        _vertices = new List<float>();
        _colors = new List<float>();
        _texCoords = new List<float>();
    }

    public void Add(float x, float y, float w, float h, Rect uv, Color color)
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

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

        _colors.Add(color.Rf);
        _colors.Add(color.Gf);
        _colors.Add(color.Bf);
        _colors.Add(color.Af);

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
    }
}