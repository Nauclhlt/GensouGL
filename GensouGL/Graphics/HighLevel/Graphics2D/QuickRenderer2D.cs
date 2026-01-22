namespace GensouGL.Graphics;

/// <summary>
/// Provides quick rendering features, which generates one draw call per object.
/// </summary>
public sealed class QuickRenderer2D : Renderer2D
{
    private QuickRenderer2D()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="QuickRenderer2D"/> with the specified screen.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public QuickRenderer2D(Screen screen)
    {
        Setup(screen);
    }

    #region Rectangle

    /// <summary>
    /// Draws the a rectangle.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect, color, color, color, color);
    }

    /// <summary>
    /// Draws the a rectangle with the colors of the four corners.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="topLeft">Top-left color of the rectangle.</param>
    /// <param name="bottomLeft">Bottom-left color of the rectangle.</param>
    /// <param name="bottomRight">Bottom-right color of the rectangle.</param>
    /// <param name="topRight">Top-right color of the rectangle.</param>
    public void DrawRect(Rect rect, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        DrawRect(rect.X, rect.Y, rect.Width, rect.Height, topLeft, bottomLeft, bottomRight, topRight);
    }

    /// <summary>
    /// Draws the a rectangle.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle.</param>
    /// <param name="y">Y coordinate of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawRect(float x, float y, float width, float height, Color color)
    {
        DrawRect(x, y, width, height, color, color, color, color);
    }

    /// <summary>
    /// Draws the a rectangle with the colors of the four corners.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle.</param>
    /// <param name="y">Y coordinate of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="topLeft">Top-left color of the rectangle.</param>
    /// <param name="bottomLeft">Bottom-left color of the rectangle.</param>
    /// <param name="bottomRight">Bottom-right color of the rectangle.</param>
    /// <param name="topRight">Top-right color of the rectangle.</param>
    public void DrawRect(float x, float y, float width, float height, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
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
            topLeft.R, topLeft.G, topLeft.B, topLeft.A,
            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,
            topRight.R, topRight.G, topRight.B, topRight.A,
            bottomRight.R, bottomRight.G, bottomRight.B, bottomRight.A,
        ];

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.TriangleStrip, va, ca);

        RestoreState();
    }

    /// <summary>
    /// Draws the a wire rectangle.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawWireRect(Rect rect, Color color)
    {
        DrawWireRect(rect, 1.0f, color, color, color, color);
    }

    /// <summary>
    /// Draws the a wire rectangle.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawWireRect(Rect rect, float lineWidth, Color color)
    {
        DrawWireRect(rect, lineWidth, color, color, color, color);
    }

    /// <summary>
    /// Draws the a wire rectangle with the colors of the four corners.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="topLeft">Top-left color of the rectangle.</param>
    /// <param name="bottomLeft">Bottom-left color of the rectangle.</param>
    /// <param name="bottomRight">Bottom-right color of the rectangle.</param>
    /// <param name="topRight">Top-right color of the rectangle.</param>
    public void DrawWireRect(Rect rect, float lineWidth, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        DrawWireRect(rect.X, rect.Y, rect.Width, rect.Height, lineWidth, topLeft, bottomLeft, bottomRight, topRight);
    }

    /// <summary>
    /// Draws the a wire rectangle.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle.</param>
    /// <param name="y">Y coordinate of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawWireRect(float x, float y, float width, float height, Color color)
    {
        DrawWireRect(x, y, width, height, 1.0f, color, color, color, color);
    }

    /// <summary>
    /// Draws the a wire rectangle.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle.</param>
    /// <param name="y">Y coordinate of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void DrawWireRect(float x, float y, float width, float height, float lineWidth, Color color)
    {
        DrawWireRect(x, y, width, height, lineWidth, color, color, color, color);
    }

    /// <summary>
    /// Draws the a wire rectangle.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle.</param>
    /// <param name="y">Y coordinate of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="topLeft">Top-left color of the rectangle.</param>
    /// <param name="bottomLeft">Bottom-left color of the rectangle.</param>
    /// <param name="bottomRight">Bottom-right color of the rectangle.</param>
    /// <param name="topRight">Top-right color of the rectangle.</param>
    public void DrawWireRect(float x, float y, float width, float height, float lineWidth, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
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
            topLeft.R, topLeft.G, topLeft.B, topLeft.A,
            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,
            bottomRight.R, bottomRight.G, bottomRight.B, bottomRight.A,
            topRight.R, topRight.G, topRight.B, topRight.A,
            topLeft.R, topLeft.G, topLeft.B, topLeft.A
        ];

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        GL.LineWidth(lineWidth);

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.LineStrip, va, ca);

        RestoreState();
    }

    #endregion

    #region Line

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="color">Color of the line.</param>
    public void DrawLine(Point a, Point b, Color color)
    {
        DrawLine(a, b, 1.0f, color, color);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the line.</param>
    public void DrawLine(Point a, Point b, float lineWidth, Color color)
    {
        DrawLine(a, b, lineWidth, color, color);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
    public void DrawLine(Point a, Point b, float lineWidth, Color aColor, Color bColor)
    {
        DrawLine(a.X, a.Y, b.X, b.Y, lineWidth, aColor, bColor);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="color">Color of the line.</param>
    public void DrawLine(float ax, float ay, float bx, float by, Color color)
    {
        DrawLine(ax, ay, bx, by, 1.0f, color, color);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the line.</param>
    public void DrawLine(float ax, float ay, float bx, float by, float lineWidth, Color color)
    {
        DrawLine(ax, ay, bx, by, lineWidth, color, color);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
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
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.Lines, va, ca);

        RestoreState();
    }

    #endregion

    #region Triangle

    /// <summary>
    /// Draws a triangle.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Point C.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawTriangle(Point a, Point b, Point c, Color color)
    {
        DrawTriangle(a, b, c, color, color, color);
    }

    /// <summary>
    /// Draws a triangle.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Point C.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
    /// <param name="cColor">Color of the Point C.</param>
    public void DrawTriangle(Point a, Point b, Point c, Color aColor, Color bColor, Color cColor)
    {
        DrawTriangle(a.X, a.Y, b.X, b.Y, c.X, c.Y, aColor, bColor, cColor);
    }

    /// <summary>
    /// Draws a triangle.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="cx">X coordinate of the Point C.</param>
    /// <param name="cy">Y coordinate of the Point C.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawTriangle(float ax, float ay, float bx, float by, float cx, float cy, Color color)
    {
        DrawTriangle(ax, ay, bx, by, cx, cy, color, color, color);
    }

    /// <summary>
    /// Draws a triangle.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="cx">X coordinate of the Point C.</param>
    /// <param name="cy">Y coordinate of the Point C.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
    /// <param name="cColor">Color of the Point C.</param>
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
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A,
            cColor.R, cColor.G, cColor.B, cColor.A,
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.Triangles, va, ca);

        RestoreState();
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Point C.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawWireTriangle(Point a, Point b, Point c, Color color)
    {
        DrawWireTriangle(a, b, c, 1.0f, color, color, color);
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Point C.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawWireTriangle(Point a, Point b, Point c, float lineWidth, Color color)
    {
        DrawWireTriangle(a, b, c, lineWidth, color, color, color);
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Point C.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
    /// <param name="cColor">Color of the Point C.</param>
    public void DrawWireTriangle(Point a, Point b, Point c, float lineWidth, Color aColor, Color bColor, Color cColor)
    {
        DrawWireTriangle(a.X, a.Y, b.X, b.Y, c.X, c.Y, lineWidth, aColor, bColor, cColor);
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="cx">X coordinate of the Point C.</param>
    /// <param name="cy">Y coordinate of the Point C.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawWireTriangle(float ax, float ay, float bx, float by, float cx, float cy, Color color)
    {
        DrawWireTriangle(ax, ay, bx, by, cx, cy, 1.0f, color, color, color);
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="cx">X coordinate of the Point C.</param>
    /// <param name="cy">Y coordinate of the Point C.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="color">Color of the triangle.</param>
    public void DrawWireTriangle(float ax, float ay, float bx, float by, float cx, float cy, float lineWidth, Color color)
    {
        DrawWireTriangle(ax, ay, bx, by, cx, cy, lineWidth, color, color, color);
    }

    /// <summary>
    /// Draws a wire triangle.
    /// </summary>
    /// <param name="ax">X coordinate of the Point A.</param>
    /// <param name="ay">Y coordinate of the Point A.</param>
    /// <param name="bx">X coordinate of the Point B.</param>
    /// <param name="by">Y coordinate of the Point B.</param>
    /// <param name="cx">X coordinate of the Point C.</param>
    /// <param name="cy">Y coordinate of the Point C.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <param name="aColor">Color of the Point A.</param>
    /// <param name="bColor">Color of the Point B.</param>
    /// <param name="cColor">Color of the Point C.</param>
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
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A,
            cColor.R, cColor.G, cColor.B, cColor.A,
            aColor.R, aColor.G, aColor.B, aColor.A,
        };

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.LineStrip, va, ca);

        RestoreState();
    }

    #endregion

    #region Ellipse

    /// <summary>
    /// Draws a circle.
    /// </summary>
    /// <param name="circle">Circle.</param>
    /// <param name="color">Color of the circle.</param>
    /// <param name="div">Division count.</param>
    public void DrawCircle(Circle circle, Color color, int div = 32)
    {
        DrawCircle(circle.X, circle.Y, circle.Radius, color, div);
    }

    /// <summary>
    /// Draws a circle.
    /// </summary>
    /// <param name="center">Center point of the circle.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="color">Color of the circle.</param>
    /// <param name="div">Division count.</param>
    public void DrawCircle(Point center, float radius, Color color, int div = 32)
    {
        DrawCircle(center.X, center.Y, radius, color, div);
    }

    /// <summary>
    /// Draws a circle.
    /// </summary>
    /// <param name="centerX">X coordinate of the center point of the circle.</param>
    /// <param name="centerY">Y coordinate of the center point of the circle.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="color">Color of the circle.</param>
    /// <param name="div">Division count.</param>
    public void DrawCircle(float centerX, float centerY, float radius, Color color, int div = 32)
    {
        DrawEllipse(centerX - radius, centerY - radius, 2 * radius, 2 * radius, color, div);
    }

    /// <summary>
    /// Draws an ellipse.
    /// </summary>
    /// <param name="rect">Bounding rectangle.</param>
    /// <param name="color">Color of the ellipse.</param>
    /// <param name="div">Division count.</param>
    public void DrawEllipse(Rect rect, Color color, int div = 32)
    {
        DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height, color, div);
    }

    /// <summary>
    /// Draws an ellipse.
    /// </summary>
    /// <param name="x">X coordinate of the bounding rectangle.</param>
    /// <param name="y">Y coordinate of the bounding rectangle.</param>
    /// <param name="width">Width of the bounding rectangle.</param>
    /// <param name="height">Height of the bounding rectangle.</param>
    /// <param name="color">Color of the ellipse.</param>
    /// <param name="div">Division count.</param>
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
            ca[(i << 2)] = color.R;
            ca[(i << 2) + 1] = color.G;
            ca[(i << 2) + 2] = color.B;
            ca[(i << 2) + 3] = color.A;
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

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.TriangleFan, va, ca);

        RestoreState();
    }

    #endregion

    #region Texture

    /// <summary>
    /// Draws a texture.
    /// </summary>
    /// <param name="rect">Bounding rectangle.</param>
    /// <param name="texture">Texture to draw.</param>
    public void DrawTexture(Rect rect, ITexture texture)
    {
        DrawTexture(rect, texture, Color.White);
    }

    /// <summary>
    /// Draws a texture with a specified color tint.
    /// </summary>
    /// <param name="rect">Bounding rectangle.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="color">Color tint.</param>
    public void DrawTexture(Rect rect, ITexture texture, Color color)
    {
        DrawTexture(rect.X, rect.Y, rect.Width, rect.Height, texture, color);
    }

    /// <summary>
    /// Draws a texture.
    /// </summary>
    /// <param name="x">X coordinate of the bounding rectangle.</param>
    /// <param name="y">Y coordinate of the bounding rectangle.</param>
    /// <param name="width">Width of the bounding rectangle.</param>
    /// <param name="height">Height of the bounding rectangle.</param>
    /// <param name="texture">Texture to draw.</param>
    public void DrawTexture(float x, float y, float width, float height, ITexture texture)
    {
        DrawTexture(x, y, width, height, texture, Color.White);
    }

    /// <summary>
    /// Draws a texture with a specified color tint.
    /// </summary>
    /// <param name="x">X coordinate of the bounding rectangle.</param>
    /// <param name="y">Y coordinate of the bounding rectangle.</param>
    /// <param name="width">Width of the bounding rectangle.</param>
    /// <param name="height">Height of the bounding rectangle.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="color">Color tint.</param>
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
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A
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

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.TriangleStrip, va, ca, ta);

        TextureService.Pop();

        RestoreState();
    }

    internal void DrawTextureWithShader(Rect rect, ITexture texture, Shader shader)
    {
        texture ??= Texture.Empty;
        Color color = Color.White;
        float[] va =
        {
            rect.X, rect.Y,
            rect.X, rect.Bottom,
            rect.Right, rect.Y,
            rect.Right, rect.Bottom
        };

        float[] ca =
        {
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A
        };

        float[] ta =
        {
            texture.Coords.X, texture.Coords.Y,
            texture.Coords.X, texture.Coords.Bottom,
            texture.Coords.Right, texture.Coords.Y,
            texture.Coords.Right, texture.Coords.Bottom
        };

        LoadState();

        shader.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        TextureService.PushAndBind(texture.TextureId);

        SharedBuffers.SimpleBuffer2D.Draw(PrimitiveType.TriangleStrip, va, ca, ta);

        TextureService.Pop();

        RestoreState();
    }

    #endregion
}

internal sealed class SimpleBuffer2D : IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _tbuf;

    private bool _disposed = false;

    public SimpleBuffer2D()
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