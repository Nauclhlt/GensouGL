namespace GensouGL.Graphics;

/// <summary>
/// Represents a high-performance shape renderer that batches draw calls to minimize GPU state changes.
/// Supports basic shape types.
/// </summary>
public sealed class BatchShapeRenderer2D : Renderer2D
{
    private List<float> _vertices = new List<float>(256);
    private List<float> _colors = new List<float>(256);
    private ShapeType _type = ShapeType.Invalid;

    private BatchShapeRenderer2D()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="BatchShapeRenderer2D"/> with the specified screen.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public BatchShapeRenderer2D(Screen screen)
    {
        Setup(screen);
    }
    
    /// <summary>
    /// Begins a drawing process using the specified shape type.
    /// </summary>
    /// <param name="type">Shape type.</param>
    public void Begin(ShapeType type)
    {
        if (type == ShapeType.Invalid)
        {
            DebugLog.Error($"{nameof(BatchShapeRenderer2D)}: Invalid ShapeType passed to Begin().");
            return;
        }
        if (_type != ShapeType.Invalid)
        {
            DebugLog.Warn($"{nameof(BatchShapeRenderer2D)}: Begin() called before End().");
        }

        _type = type;
        _vertices.Clear();
        _colors.Clear();
    }

    /// <summary>
    /// Finalizes the batch and submits all accumulated data to the GPU.
    /// </summary>
    public void End()
    {
        if (_type == ShapeType.Invalid)
        {
            DebugLog.Warn($"{nameof(BatchShapeRenderer2D)}: End() called before Begin().");
            return;
        }

        if (_vertices.Count == 0)
        {
            _type = ShapeType.Invalid;
            return;
        }

        LoadState();

        Shader.SimpleShape.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        PrimitiveType primitiveType = _type.ToPrimitiveType();

        Span<float> vspan = CollectionsMarshal.AsSpan(_vertices);
        Span<float> cspan = CollectionsMarshal.AsSpan(_colors);
        SharedBuffers.SimpleBuffer2D.Draw(primitiveType, vspan, cspan);

        RestoreState();
        _type = ShapeType.Invalid;
    }

    // returns a flag indicating error
    private bool EnsureType(ShapeType type)
    {
        if (_type != type)
        {
            DebugLog.Error($"{nameof(BatchShapeRenderer2D)}: Attempted to draw shape of type {type} while batch is for type {_type}.");
            return true;
        }
        return false;
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
        if (EnsureType(ShapeType.Rect))
        {
            return;
        }

        _vertices.AddRange(
        [
            x, y,
            x, y + height,
            x + width, y,

            x + width, y,
            x, y + height,
            x + width, y + height
        ]);

        _colors.AddRange(
        [
            topLeft.R, topLeft.G, topLeft.B, topLeft.A,
            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,
            topRight.R, topRight.G, topRight.B, topRight.A,

            topRight.R, topRight.G, topRight.B, topRight.A,
            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,
            bottomRight.R, bottomRight.G, bottomRight.B, bottomRight.A,
        ]);
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
        if (EnsureType(ShapeType.WireRect))
        {
            return;
        }

        _vertices.AddRange(
        [
            x, y,
            x, y + height,

            x, y + height,
            x + width, y + height,

            x + width, y + height,
            x + width, y,

            x + width, y,
            x, y
        ]);

        _colors.AddRange(
        [
            topLeft.R, topLeft.G, topLeft.B, topLeft.A,
            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,

            bottomLeft.R, bottomLeft.G, bottomLeft.B, bottomLeft.A,
            bottomRight.R, bottomRight.G, bottomRight.B, bottomRight.A,

            bottomRight.R, bottomRight.G, bottomRight.B, bottomRight.A,
            topRight.R, topRight.G, topRight.B, topRight.A,

            topRight.R, topRight.G, topRight.B, topRight.A,
            topLeft.R, topLeft.G, topLeft.B, topLeft.A
        ]);
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
        if (EnsureType(ShapeType.Line))
        {
            return;
        }

        GL.LineWidth(lineWidth);

        _vertices.AddRange(
        [
            ax, ay,
            bx, by
        ]);

        _colors.AddRange(
        [
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A
        ]);
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
        if (EnsureType(ShapeType.Triangle))
        {
            return;
        }

        _vertices.AddRange(
        [
            ax, ay,
            bx, by,
            cx, cy
        ]);

        _colors.AddRange(
        [
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A,
            cColor.R, cColor.G, cColor.B, cColor.A,
        ]);
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
        if (EnsureType(ShapeType.WireTriangle))
        {
            return;
        }

        GL.LineWidth(lineWidth);

        _vertices.AddRange(
        [
            ax, ay,
            bx, by,

            bx, by,
            cx, cy,

            cx, cy,
            ax, ay
        ]);

        _colors.AddRange(
        [
            aColor.R, aColor.G, aColor.B, aColor.A,
            bColor.R, bColor.G, bColor.B, bColor.A,

            bColor.R, bColor.G, bColor.B, bColor.A,
            cColor.R, cColor.G, cColor.B, cColor.A,

            cColor.R, cColor.G, cColor.B, cColor.A,
            aColor.R, aColor.G, aColor.B, aColor.A,
        ]);
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
    /// <param name="center">Center point of the ellipse.</param>
    /// <param name="radiusX">X radius.</param>
    /// <param name="radiusY">Y radius.</param>
    /// <param name="color">Color of the ellipse.</param>
    /// <param name="div">Division count.</param>
    public void DrawEllipse(Point center, float radiusX, float radiusY, Color color, int div = 32)
    {
        DrawEllipse(center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2, color, div);
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
        if (EnsureType(ShapeType.Ellipse))
        {
            return;
        }

        float centerX = x + width / 2;
        float centerY = y + height / 2;
        float radX = width / 2;
        float radY = height / 2;

        int v = div + 2;
        int cur = 1;

        float divAngle = MathF.PI * 2 / div;

        for (int i = 0; i < div; i++)
        {
            _vertices.Add(centerX);
            _vertices.Add(centerY);

            _vertices.Add(centerX + radX * MathF.Cos(i * divAngle));
            _vertices.Add(centerY + radY * MathF.Sin(i * divAngle));

            _vertices.Add(centerX + radX * MathF.Cos((i + 1) * divAngle));
            _vertices.Add(centerY + radY * MathF.Sin((i + 1) * divAngle));

            _colors.AddRange([
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
            ]);

            cur++;
        }
    }

    #endregion
}