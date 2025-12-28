namespace GensouGL.Graphics;

public sealed class BatchShapeRenderer : Renderer
{
    private List<float> _vertices = [];
    private List<float> _colors = [];
    private ShapeType _type = ShapeType.Invalid;

    private BatchShapeRenderer()
    {
    }

    internal BatchShapeRenderer(Game game, Screen screen)
    {
        Setup(game, screen);
    }

    public void Begin(ShapeType type)
    {
        if (type == ShapeType.Invalid)
        {
#if DEBUG
            DebugLog.Error("BatchShapeRenderer: Invalid ShapeType passed to Begin().");
#endif
            return;
        }
        if (_type != ShapeType.Invalid)
        {
#if DEBUG
            DebugLog.Warn("BatchShapeRenderer: Begin() called before End().");
#endif
        }

        _type = type;
        _vertices.Clear();
        _colors.Clear();
    }

    public void End()
    {
        if (_type == ShapeType.Invalid)
        {
#if DEBUG
            DebugLog.Warn("BatchShapeRenderer: End() called before Begin().");
#endif
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
        QuickRenderer.InternalBuffer.Draw(primitiveType, vspan, cspan);

        RestoreState();
        _type = ShapeType.Invalid;
    }

    // returns a flag indicating an error
    private bool EnsureType(ShapeType type)
    {
        if (_type != type)
        {
#if DEBUG
            DebugLog.Error($"BatchShapeRenderer: Attempted to draw shape of type {type} while batch is for type {_type}.");
#endif
            return true;
        }
        return false;
    }

    #region Rectangle

    public void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect, color, color, color, color);
    }

    public void DrawRect(Rect rect, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        DrawRect(rect.X, rect.Y, rect.Width, rect.Height, topLeft, bottomLeft, bottomRight, topRight);
    }

    public void DrawRect(float x, float y, float width, float height, Color color)
    {
        DrawRect(x, y, width, height, color, color, color, color);
    }

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

    public void DrawWireRect(Rect rect, Color color)
    {
        DrawWireRect(rect, 1.0f, color, color, color, color);
    }

    public void DrawWireRect(Rect rect, float lineWidth, Color color)
    {
        DrawWireRect(rect, lineWidth, color, color, color, color);
    }

    public void DrawWireRect(Rect rect, float lineWidth, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        DrawWireRect(rect.X, rect.Y, rect.Width, rect.Height, lineWidth, topLeft, bottomLeft, bottomRight, topRight);
    }

    public void DrawWireRect(float x, float y, float width, float height, Color color)
    {
        DrawWireRect(x, y, width, height, 1.0f, color, color, color, color);
    }

    public void DrawWireRect(float x, float y, float width, float height, float lineWidth, Color color)
    {
        DrawWireRect(x, y, width, height, lineWidth, color, color, color, color);
    }

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