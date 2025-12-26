namespace GensouGL.Core;

public struct RectGradient
{
    public Color TopLeft;
    public Color BottomLeft;
    public Color BottomRight;
    public Color TopRight;
    

    public RectGradient(Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
    }

    public RectGradient(Color color)
    {
        TopLeft = color;
        TopRight = color;
        BottomLeft = color;
        BottomRight = color;
    }

    public static RectGradient Vertical(Color top, Color bottom)
    {
        return new RectGradient(top, bottom, bottom, top);
    }

    public static RectGradient Horizontal(Color left, Color right)
    {
        return new RectGradient(left, left, right, right);
    }
}