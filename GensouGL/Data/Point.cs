namespace GensouGL.Core;

[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    public static readonly Point Origin = new(0, 0);

    public float X;
    public float Y;

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly float Abs => MathF.Sqrt(X * X + Y * Y);

    public readonly Point Offset(float dx, float dy) => new(X + dx, Y + dy);

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static Point operator *(Point a, float b) => new(a.X * b, a.Y * b);
    public static Point operator /(Point a, float b) => new(a.X / b, a.Y / b);
}