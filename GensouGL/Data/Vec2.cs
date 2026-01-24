namespace GensouGL.Core;

/// <summary>
/// Represents a 2D vector.
/// </summary>
public struct Vec2
{
    /// <summary>
    /// X component.
    /// </summary>
    public float X;
    /// <summary>
    /// Y component.
    /// </summary>
    public float Y;

    /// <summary>
    /// Initializes <see cref="Vec2"/> using the specified X and Y components.
    /// </summary>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes <see cref="Vec2"/> using the specified X and Y components.
    /// </summary>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes <see cref="Vec2"/> with both components set to the specified value.
    /// </summary>
    /// <param name="component">Value for all components.</param>
    public Vec2(float component)
    {
        X = component;
        Y = component;
    }

    /// <summary>
    /// Initializes <see cref="Vec2"/> representing the specified point.
    /// </summary>
    /// <param name="point">Source point.</param>
    public Vec2(Point point)
    {
        X = point.X;
        Y = point.Y;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Vec2 Zero => new (0, 0);
    /// <summary>
    /// Gets the vector (1, 0);
    /// </summary>
    public static Vec2 UnitX => new (1, 0);
    /// <summary>
    /// Gets the vector (0, 1);
    /// </summary>
    public static Vec2 UnitY => new (0, 1);

    /// <summary>
    /// Gets the length of the vector |(X, Y)|;
    /// </summary>
    public readonly float Length => MathF.Sqrt(X * X + Y * Y);
    /// <summary>
    /// Gets the squared length of the vector |(X, Y)|^2.
    /// </summary>
    public readonly float LengthSquared => X * X + Y * Y;
    /// <summary>
    /// Gets the value of atan2(Y, X).
    /// </summary>
    public readonly float Angle => MathF.Atan2(Y, X);

    /// <summary>
    /// Gets the vector rotated the specified angle in radians around the origin.
    /// </summary>
    /// <param name="angleRadians">Rotation angle in radians.</param>
    /// <returns>Rotated vector.</returns>
    public readonly Vec2 Rotate(float angleRadians)
    {
        float cos = MathF.Cos(angleRadians);
        float sin = MathF.Sin(angleRadians);
        return new Vec2(
            X * cos - Y * sin,
            X * sin + Y * cos
        );
    }

    /// <summary>
    /// Gets the normalized vector (X, Y) / |(X, Y)|.
    /// </summary>
    /// <returns>Normalized vector.</returns>
    public readonly Vec2 Normalize()
    {
        float length = Length;
        if (length == 0) return new Vec2(0, 0);
        return new Vec2(X / length, Y / length);
    }

    /// <summary>
    /// Gets the dot product of the current vector and specified vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Dot product.</returns>
    public readonly float Dot(Vec2 other)
    {
        return X * other.X + Y * other.Y;
    }

    /// <summary>
    /// Gets the vector representing the cross product of the current vector and specified vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Cross product.</returns>
    public readonly Vec2 Cross(Vec2 other)
    {
        return new Vec2(
            Y * other.X - X * other.Y,
            X * other.Y - Y * other.X
        );
    }

    internal readonly OpenTK.Mathematics.Vector2 ToOpenTK()
    {
        return new OpenTK.Mathematics.Vector2(X, Y);
    }

    /// <summary>
    /// Adds 2 vectors.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec2 operator +(Vec2 a, Vec2 b) => new (a.X + b.X, a.Y + b.Y);
    /// <summary>
    /// Subtracts vector B from vector A.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec2 operator -(Vec2 a, Vec2 b) => new (a.X - b.X, a.Y - b.Y);
    /// <summary>
    /// Multiplies the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec2 operator *(Vec2 v, float scalar) => new (v.X * scalar, v.Y * scalar);
    /// <summary>
    /// Multiplies the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec2 operator *(float scalar, Vec2 v) => new(v.X * scalar, v.Y * scalar);
    /// <summary>
    /// Divides the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec2 operator /(Vec2 v, float scalar) => new (v.X / scalar, v.Y / scalar);

    /// <summary>
    /// Gets the vector (-X, -Y).
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec2 operator -(Vec2 v) => new(-v.X, -v.Y);

    /// <summary>
    /// Converts the vector into Point.
    /// </summary>
    /// <param name="v">Source vector.</param>
    public static implicit operator Point(Vec2 v) => new(v.X, v.Y);
    /// <summary>
    /// Converts Point into the vector.
    /// </summary>
    /// <param name="p"></param>
    public static implicit operator Vec2(Point p) => new(p.X, p.Y);
}
