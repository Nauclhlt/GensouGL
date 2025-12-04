namespace GensouGL.Core;

public struct GVector2
{
    public float X;
    public float Y;

    public GVector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public GVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public GVector2(Point point)
    {
        X = point.X;
        Y = point.Y;
    }

    public static GVector2 Zero => new (0, 0);
    public static GVector2 UnitX => new (1, 0);
    public static GVector2 UnitY => new (0, 1);

    public readonly float Length => MathF.Sqrt(X * X + Y * Y);
    public readonly float LengthSquared => X * X + Y * Y;
    public readonly float Angle => MathF.Atan2(Y, X);

    public readonly GVector2 Rotate(float angleRadians)
    {
        float cos = MathF.Cos(angleRadians);
        float sin = MathF.Sin(angleRadians);
        return new GVector2(
            X * cos - Y * sin,
            X * sin + Y * cos
        );
    }

    public readonly GVector2 Normalize()
    {
        float length = Length;
        if (length == 0) return new GVector2(0, 0);
        return new GVector2(X / length, Y / length);
    }

    public readonly float Dot(GVector2 other)
    {
        return X * other.X + Y * other.Y;
    }

    public readonly GVector2 Cross(GVector2 other)
    {
        return new GVector2(
            Y * other.X - X * other.Y,
            X * other.Y - Y * other.X
        );
    }

    internal readonly OpenTK.Mathematics.Vector2 ToOpenTK()
    {
        return new OpenTK.Mathematics.Vector2(X, Y);
    }

    public static GVector2 operator +(GVector2 a, GVector2 b) => new (a.X + b.X, a.Y + b.Y);
    public static GVector2 operator -(GVector2 a, GVector2 b) => new (a.X - b.X, a.Y - b.Y);
    public static GVector2 operator *(GVector2 v, float scalar) => new (v.X * scalar, v.Y * scalar);
    public static GVector2 operator /(GVector2 v, float scalar) => new (v.X / scalar, v.Y / scalar);
}
