namespace GensouGL.Core;

[StructLayout(LayoutKind.Sequential)]
public struct Circle
{
    public float X;
    public float Y;
    public float Radius;

    public Circle(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public readonly Point Point => new(X, Y);
    public readonly Rect Bounds => new(X - Radius, Y - Radius, Radius * 2, Radius * 2);

    public readonly Circle Offset(float offsetX, float offsetY)
    {
        return new Circle(X + offsetX, Y + offsetY, Radius);
    }

    public readonly Circle Resize(float newRadius)
    {
        return new Circle(X, Y, newRadius);
    }

    public readonly bool Contains(Point point)
    {
        float dx = point.X - X;
        float dy = point.Y - Y;
        return (dx * dx + dy * dy) <= (Radius * Radius);
    }

    public readonly bool Intersects(Circle other)
    {
        float dx = other.X - X;
        float dy = other.Y - Y;
        float radiiSum = Radius + other.Radius;
        return (dx * dx + dy * dy) <= (radiiSum * radiiSum);
    }

    public readonly bool Contains(Rect rect)
    {
        return Contains(new Point(rect.X, rect.Y)) &&
               Contains(new Point(rect.X + rect.Width, rect.Y)) &&
               Contains(new Point(rect.X, rect.Y + rect.Height)) &&
               Contains(new Point(rect.X + rect.Width, rect.Y + rect.Height));
    }
}