namespace GensouGL.Core;

/// <summary>
/// Represents a 3D vector.
/// </summary>
public struct Vec3
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
    /// Z component.
    /// </summary>
    public float Z;

    /// <summary>
    /// Initializes <see cref="Vec3"/> using the specified X, Y, and Z components.
    /// </summary>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    /// <param name="z">Z component.</param>
    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Initializes <see cref="Vec3"/> using the specified X, Y and Z components.
    /// </summary>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    /// <param name="z">Z component.</param>
    public Vec3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    /// <summary>
    /// Initializes <see cref="Vec3"/> using the specified 2D vector and z component.
    /// </summary>
    /// <param name="xy">XY vector.</param>
    /// <param name="z">Z component.</param>
    public Vec3(Vec2 xy, float z)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Vec3 Zero => new(0, 0, 0);
    /// <summary>
    /// Gets the vector (1, 0, 0).
    /// </summary>
    public static Vec3 UnitX => new(1, 0, 0);
    /// <summary>
    /// Gets the vector (0, 1, 0).
    /// </summary>
    public static Vec3 UnitY => new(0, 1, 0);
    /// <summary>
    /// Gets the vector (0, 0, 1).
    /// </summary>
    public static Vec3 UnitZ => new(0, 0, 1);

    /// <summary>
    /// Gets the length of the vector |(X, Y, Z)|;
    /// </summary>
    public readonly float Length => MathF.Sqrt(X * X + Y * Y + Z * Z);
    /// <summary>
    /// Gets the squared length of the vector |(X, Y, Z)|^2.
    /// </summary>
    public readonly float LengthSquared => X * X + Y * Y + Z * Z;

    /// <summary>
    /// Returns the vector (X, Y).
    /// </summary>
    public readonly Vec2 XY => new Vec2(X, Y);
    /// <summary>
    /// Returns the vector (Y, Z).
    /// </summary>
    public readonly Vec2 YZ => new Vec2(Y, Z);
    /// <summary>
    /// Returns the vector (Z, X).
    /// </summary>
    public readonly Vec2 XZ => new Vec2(X, Z);

    /// <summary>
    /// Gets the vector rotated the specified angle in radians around the X-axis.
    /// </summary>
    /// <param name="angleRadians">Rotation angle in radians.</param>
    /// <returns>Rotated vector.</returns>
    public readonly Vec3 RotateX(float angleRadians)
    {
        float sin = MathF.Sin(angleRadians);
        float cos = MathF.Cos(angleRadians);

        return new Vec3(X,
            Y * cos - Z * sin,
            Y * sin + Z * cos);
    }

    /// <summary>
    /// Gets the vector rotated the specified angle in radians around the Y-axis.
    /// </summary>
    /// <param name="angleRadians">Rotation angle in radians.</param>
    /// <returns>Rotated vector.</returns>
    public readonly Vec3 RotateY(float angleRadians)
    {
        float sin = MathF.Sin(angleRadians);
        float cos = MathF.Cos(angleRadians);

        return new Vec3(X * cos - Z * sin,
            Y,
            X * sin + Z * cos);
    }

    /// <summary>
    /// Gets the vector rotated the specified angle in radians around the Z-axis.
    /// </summary>
    /// <param name="angleRadians">Rotation angle in radians.</param>
    /// <returns>Rotated vector.</returns>
    public readonly Vec3 RotateZ(float angleRadians)
    {
        float sin = MathF.Sin(angleRadians);
        float cos = MathF.Cos(angleRadians);

        return new Vec3(X * cos - Y * sin,
            X * sin + Y * cos,
            Z);
    }

    /// <summary>
    /// Gets the normalized vector (X, Y, Z) / |(X, Y, Z)|.
    /// </summary>
    /// <returns>Normalized vector.</returns>
    public readonly Vec3 Normalize()
    {
        float length = Length;
        if (length == 0f) return Zero;
        return new Vec3(X / length, Y / length, Z / length);
    }

    /// <summary>
    /// Gets the dot product of the current vector and specified vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Dot product.</returns>
    public readonly float Dot(Vec3 other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    /// <summary>
    /// Gets the vector representing the cross product of the current vector and specified vector.
    /// </summary>
    /// <param name="other">Target vector.</param>
    /// <returns>Cross product.</returns>
    public readonly Vec3 Cross(Vec3 other)
    {
        return new Vec3(
            Y * other.Z - other.Y * Z,
            Z * other.X - other.Z * X,
            X * other.Y - other.X * Y
        );
    }

    /// <summary>
    /// Gets the dot product of the vector A and B.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Dot product.</returns>
    public static float Dot(Vec3 a, Vec3 b)
    {
        return a.Dot(b);
    }

    /// <summary>
    /// Gets the vector representing the cross product of the vector A and B.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Cross product.</returns>
    public static Vec3 Cross(Vec3 a, Vec3 b)
    {
        return a.Cross(b);
    }

    internal readonly OpenTK.Mathematics.Vector3 ToOpenTK()
    {
        return new OpenTK.Mathematics.Vector3(X, Y, Z);
    }

    /// <summary>
    /// Adds 2 vectors.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    /// <summary>
    /// Subtracts vector B from vector A.
    /// </summary>
    /// <param name="a">Vector A.</param>
    /// <param name="b">Vector B.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    /// <summary>
    /// Multiplies the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec3 operator *(Vec3 v, float scalar) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);
    /// <summary>
    /// Multiplies the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec3 operator *(float scalar, Vec3 v) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);
    /// <summary>
    /// Divides the vector by the scalar.
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <param name="scalar">Scalar.</param>
    /// <returns>Vector*Scalar.</returns>
    public static Vec3 operator /(Vec3 v, float scalar) => new(v.X / scalar, v.Y / scalar, v.Z / scalar);

    /// <summary>
    /// Gets the vector (-X, -Y, -Z).
    /// </summary>
    /// <param name="v">Vector.</param>
    /// <returns>Resulting vector.</returns>
    public static Vec3 operator -(Vec3 v) => new(-v.X, -v.Y, -v.Z);
}
