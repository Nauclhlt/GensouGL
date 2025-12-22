namespace GensouGL.Graphics;

public sealed class QuickRenderer : Renderer
{
    private static QuickRendererBuffer _buffer;

    internal static QuickRendererBuffer InternalBuffer => _buffer;

    internal static void Initialize()
    {
        _buffer = new QuickRendererBuffer();
    }

    internal static void Release()
    {
        _buffer.Dispose();
    }






    private QuickRenderer()
    {
    }

    internal QuickRenderer(Game game, Screen screen)
    {
        Setup(game, screen);
    }

    #region Rectangle

    public void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect, color, color, color, color);
    }

    public void DrawRect(Rect rect, Color upperLeft, Color lowerLeft, Color lowerRight, Color upperRight)
    {
        DrawRect(rect.X, rect.Y, rect.Width, rect.Height, upperLeft, lowerLeft, lowerRight, upperRight);
    }

    public void DrawRect(float x, float y, float width, float height, Color color)
    {
        DrawRect(x, y, width, height, color, color, color, color);
    }

    public void DrawRect(float x, float y, float width, float height, Color upperLeft, Color lowerLeft, Color lowerRight, Color upperRight)
    {
        float[] va =
        [
            x, y,
            x, y + height,
            x + width, y,
            x + width, y + height
        ];

        float[] ca =
        [
            upperLeft.Rf, upperLeft.Gf, upperLeft.Bf, upperLeft.Af,
            lowerLeft.Rf, lowerLeft.Gf, lowerLeft.Bf, lowerLeft.Af,
            upperRight.Rf, upperRight.Gf, upperRight.Bf, upperRight.Af,
            lowerRight.Rf, lowerRight.Gf, lowerRight.Bf, lowerRight.Af,
        ];

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        _buffer.Draw(PrimitiveType.TriangleStrip, va, ca);

        RestoreState();
    }

    public void DrawWireRect(Rect rect, Color color)
    {
        DrawWireRect(rect, 1.0f, color, color, color, color);
    }

    public void DrawWireRect(Rect rect, float lineWidth, Color color)
    {
        DrawWireRect(rect, lineWidth, color, color, color, color);
    }

    public void DrawWireRect(Rect rect, float lineWidth, Color upperLeft, Color lowerLeft, Color lowerRight, Color upperRight)
    {
        DrawWireRect(rect.X, rect.Y, rect.Width, rect.Height, lineWidth, upperLeft, lowerLeft, lowerRight, upperRight);
    }

    public void DrawWireRect(float x, float y, float width, float height, Color color)
    {
        DrawWireRect(x, y, width, height, 1.0f, color, color, color, color);
    }

    public void DrawWireRect(float x, float y, float width, float height, float lineWidth, Color color)
    {
        DrawWireRect(x, y, width, height, lineWidth, color, color, color, color);
    }

    public void DrawWireRect(float x, float y, float width, float height, float lineWidth, Color upperLeft, Color lowerLeft, Color lowerRight, Color upperRight)
    {
        float[] va =
        [
            x, y,
            x, y + height,
            x + width, y + height,
            x + width, y,
            x, y
        ];

        float[] ca =
        [
            upperLeft.Rf, upperLeft.Gf, upperLeft.Bf, upperLeft.Af,
            lowerLeft.Rf, lowerLeft.Gf, lowerLeft.Bf, lowerLeft.Af,
            lowerRight.Rf, lowerRight.Gf, lowerRight.Bf, lowerRight.Af,
            upperRight.Rf, upperRight.Gf, upperRight.Bf, upperRight.Af,
            upperLeft.Rf, upperLeft.Gf, upperLeft.Bf, upperLeft.Af
        ];

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        GL.LineWidth(lineWidth);

        _buffer.Draw(PrimitiveType.LineStrip, va, ca);

        RestoreState();
    }

    #endregion

    #region Line

    public void DrawLine(Point a, Point b, Color color)
    {
        DrawLine(a, b, 1.0f, color, color);
    }

    public void DrawLine(Point a, Point b, float lineWidth, Color color)
    {
        DrawLine(a, b, lineWidth, color, color);
    }

    public void DrawLine(Point a, Point b, float lineWidth, Color aColor, Color bColor)
    {
        DrawLine(a.X, a.Y, b.X, b.Y, lineWidth, aColor, bColor);
    }

    public void DrawLine(float ax, float ay, float bx, float by, Color color)
    {
        DrawLine(ax, ay, bx, by, 1.0f, color, color);
    }

    public void DrawLine(float ax, float ay, float bx, float by, float lineWidth, Color color)
    {
        DrawLine(ax, ay, bx, by, lineWidth, color, color);
    }

    public void DrawLine(float ax, float ay, float bx, float by, float lineWidth, Color aColor, Color bColor)
    {
        GL.LineWidth(lineWidth);

        float[] va =
        {
            ax, ay,
            bx, by
        };

        float[] ca =
        {
            aColor.Rf, aColor.Gf, aColor.Bf, aColor.Af,
            bColor.Rf, bColor.Gf, bColor.Bf, bColor.Af
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        _buffer.Draw(PrimitiveType.Lines, va, ca);

        RestoreState();
    }

    #endregion

    #region Triangle

    public void DrawTriangle(Point a, Point b, Point c, Color color)
    {
        DrawTriangle(a, b, c, color, color, color);
    }

    public void DrawTriangle(Point a, Point b, Point c, Color aColor, Color bColor, Color cColor)
    {
        DrawTriangle(a.X, a.Y, b.X, b.Y, c.X, c.Y, aColor, bColor, cColor);
    }

    public void DrawTriangle(float ax, float ay, float bx, float by, float cx, float cy, Color color)
    {
        DrawTriangle(ax, ay, bx, by, cx, cy, color, color, color);
    }

    public void DrawTriangle(float ax, float ay, float bx, float by, float cx, float cy, Color aColor, Color bColor, Color cColor)
    {
        float[] va =
        {
            ax, ay,
            bx, by,
            cx, cy
        };

        float[] ca =
        {
            aColor.Rf, aColor.Gf, aColor.Bf, aColor.Af,
            bColor.Rf, bColor.Gf, bColor.Bf, bColor.Af,
            cColor.Rf, cColor.Gf, cColor.Bf, cColor.Af,
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        _buffer.Draw(PrimitiveType.Triangles, va, ca);

        RestoreState();
    }

    public void DrawWireTriangle(Point a, Point b, Point c, Color color)
    {
        DrawWireTriangle(a, b, c, 1.0f, color, color, color);
    }

    public void DrawWireTriangle(Point a, Point b, Point c, float lineWidth, Color color)
    {
        DrawWireTriangle(a, b, c, lineWidth, color, color, color);
    }

    public void DrawWireTriangle(Point a, Point b, Point c, float lineWidth, Color aColor, Color bColor, Color cColor)
    {
        DrawWireTriangle(a.X, a.Y, b.X, b.Y, c.X, c.Y, lineWidth, aColor, bColor, cColor);
    }

    public void DrawWireTriangle(float ax, float ay, float bx, float by, float cx, float cy, Color color)
    {
        DrawWireTriangle(ax, ay, bx, by, cx, cy, 1.0f, color, color, color);
    }

    public void DrawWireTriangle(float ax, float ay, float bx, float by, float cx, float cy, float lineWidth, Color color)
    {
        DrawWireTriangle(ax, ay, bx, by, cx, cy, lineWidth, color, color, color);
    }

    public void DrawWireTriangle(float ax, float ay, float bx, float by, float cx, float cy, float lineWidth, Color aColor, Color bColor, Color cColor)
    {
        GL.LineWidth(lineWidth);

        float[] va =
        {
            ax, ay,
            bx, by,
            cx, cy,
            ax, ay
        };

        float[] ca =
        {
            aColor.Rf, aColor.Gf, aColor.Bf, aColor.Af,
            bColor.Rf, bColor.Gf, bColor.Bf, bColor.Af,
            cColor.Rf, cColor.Gf, cColor.Bf, cColor.Af,
            aColor.Rf, aColor.Gf, aColor.Bf, aColor.Af,
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        _buffer.Draw(PrimitiveType.LineStrip, va, ca);

        RestoreState();
    }

    #endregion

    #region Ellipse

    public void DrawCircle(Circle circle, Color color, int div = 32)
    {
        DrawCircle(circle.X, circle.Y, circle.Radius, color, div);
    }

    public void DrawCircle(Point center, float radius, Color color, int div = 32)
    {
        DrawCircle(center.X, center.Y, radius, color, div);
    }

    public void DrawCircle(float centerX, float centerY, float radius, Color color, int div = 32)
    {
        DrawEllipse(centerX - radius, centerY - radius, 2 * radius, 2 * radius, color, div);
    }

    public void DrawEllipse(Rect rect, Color color, int div = 32)
    {
        DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height, color, div);
    }

    public void DrawEllipse(float x, float y, float width, float height, Color color, int div = 32)
    {
        float centerX = x + width / 2;
        float centerY = y + height / 2;
        float radX = width / 2;
        float radY = height / 2;

        int v = div + 2;
        int cur = 1;

        float[] va = new float[v * 2];
        float[] ca = new float[v * 4];
        for (int i = 0; i < v; i++)
        {
            ca[(i << 2)] = color.Rf;
            ca[(i << 2) + 1] = color.Gf;
            ca[(i << 2) + 2] = color.Bf;
            ca[(i << 2) + 3] = color.Af;
        }
        va[0] = centerX;
        va[1] = centerY;

        float divAngle = MathF.PI * 2 / div;

        for (int i = 0; i <= div; i++)
        {
            va[cur << 1] = centerX + radX * MathF.Cos(i * divAngle);
            va[(cur << 1) + 1] = centerY + radY * MathF.Sin(i * divAngle);
            cur++;
        }

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        _buffer.Draw(PrimitiveType.TriangleFan, va, ca);

        RestoreState();
    }

    #endregion

    #region Texture

    public void DrawTexture(Rect rect, ITexture texture)
    {
        DrawTexture(rect, texture, Color.White);
    }

    public void DrawTexture(Rect rect, ITexture texture, Color color)
    {
        DrawTexture(rect.X, rect.Y, rect.Width, rect.Height, texture, color);
    }

    public void DrawTexture(float x, float y, float width, float height, ITexture texture)
    {
        DrawTexture(x, y, width, height, texture, Color.White);
    }

    public void DrawTexture(float x, float y, float width, float height, ITexture texture, Color color)
    {
        texture ??= Texture.Empty;
        float[] va =
        {
            x, y,
            x, y + height,
            x + width, y,
            x + width, y + height
        };

        float[] ca =
        {
            color.Rf, color.Gf, color.Bf, color.Af,
            color.Rf, color.Gf, color.Bf, color.Af,
            color.Rf, color.Gf, color.Bf, color.Af,
            color.Rf, color.Gf, color.Bf, color.Af
        };

        float[] ta =
        {
            texture.Coords.X, texture.Coords.Y,
            texture.Coords.X, texture.Coords.Bottom,
            texture.Coords.Right, texture.Coords.Y,
            texture.Coords.Right, texture.Coords.Bottom
        };

        LoadState();

        Shader.SimpleTexture.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        TextureService.PushAndBind(texture.TextureId);

        _buffer.Draw(PrimitiveType.TriangleStrip, va, ca, ta);

        TextureService.Pop();

        RestoreState();
    }

    #endregion
}

internal sealed class QuickRendererBuffer : IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _tbuf;

    private bool _disposed = false;

    public QuickRendererBuffer()
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

    public void Draw(PrimitiveType type, Span<float> va, Span<float> ca, Span<float> ta)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Length, ref va[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ca.Length, ref ca[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ta.Length, ref ta[0], BufferUsageHint.DynamicDraw);


        GL.BindVertexArray(_vao);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.DrawArrays(type, 0, va.Length / 2);

        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, Span<float> va, Span<float> ca)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Length, ref va[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ca.Length, ref ca[0], BufferUsageHint.DynamicDraw);

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.DrawArrays(type, 0, va.Length / 2);

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