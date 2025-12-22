namespace GensouGL.Core;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public readonly Point Point => new(X, Y);
    
    public Rect(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public readonly float Area => Width * Height;
    public readonly float Left => X;
    public readonly float Top => Y;
    public readonly float Right => X + Width;
    public readonly float Bottom => Y + Height;
    public readonly Point CenterPoint => new(X + Width / 2, Y + Height / 2);
    public readonly bool HasArea => Width != 0 && Height != 0;

    public readonly Rect Offset(float offsetX, float offsetY)
    {
        return new Rect(X + offsetX, Y + offsetY, Width, Height);
    }

    public readonly Rect Resize(float newWidth, float newHeight)
    {
        return new Rect(X, Y, newWidth, newHeight);
    }

    public readonly Rect Center(float width, float height)
    {
        return new Rect(X + (Width - width) / 2f, Y + (Height - height) / 2f, width, height);
    }

    public readonly Rect GetPartialRelative(Rect rect)
    {
        return new Rect(
            X + rect.X * Width,
            Y + rect.Y * Height,
            rect.Width * Width,
            rect.Height * Height
        );
    }

    public readonly bool Contains(Point point)
    {
        return point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
    }

    public readonly bool Contains(Rect rect)
    {
        return rect.Left >= Left && rect.Right <= Right && rect.Top >= Top && rect.Bottom <= Bottom;
    }

    public readonly bool Contains(Circle circle)
    {
        return circle.X - circle.Radius >= Left &&
               circle.X + circle.Radius <= Right &&
               circle.Y - circle.Radius >= Top &&
               circle.Y + circle.Radius <= Bottom;
    }

    public readonly bool Intersects(Rect rect)
    {
        return !(rect.Left > Right || rect.Right < Left || rect.Top > Bottom || rect.Bottom < Top);
    }

    public readonly bool Intersects(Circle circle)
    {
        float nearestX = MathF.Max(Left, MathF.Min(circle.X, Right));
        float nearestY = MathF.Max(Top, MathF.Min(circle.Y, Bottom));
        float deltaX = circle.X - nearestX;
        float deltaY = circle.Y - nearestY;
        return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
    }
}