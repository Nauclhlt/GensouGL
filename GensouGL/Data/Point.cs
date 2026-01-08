namespace GensouGL.Core;

/// <summary>
/// Represents a 2D point.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    /// <summary>
    /// Gets the point (0, 0).
    /// </summary>
    public static readonly Point Origin = new(0, 0);

    /// <summary>
    /// X coordinate.
    /// </summary>
    public float X;
    /// <summary>
    /// Y coordinate.
    /// </summary>
    public float Y;

    /// <summary>
    /// Initializes <see cref="Point"/> using the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes <see cref="Point"/> using the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Returns the value |(X, Y)|.
    /// </summary>
    public readonly float Abs => MathF.Sqrt(X * X + Y * Y);

    /// <summary>
    /// Gets the point (X + dx, Y + dy).
    /// </summary>
    /// <param name="dx">X offset.</param>
    /// <param name="dy">Y offset.</param>
    /// <returns>Translated point.</returns>
    public readonly Point Offset(float dx, float dy) => new(X + dx, Y + dy);

    /// <summary>
    /// Adds 2 points.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <returns>Resulting point.</returns>
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    /// <summary>
    /// Subtracts point B from point A.
    /// </summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <returns>Resulting point.</returns>
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    /// <summary>
    /// Multiplies the point by the scalar.
    /// </summary>
    /// <param name="a">Point.</param>
    /// <param name="b">Scalar.</param>
    /// <returns>Resulting point.</returns>
    public static Point operator *(Point a, float b) => new(a.X * b, a.Y * b);
    /// <summary>
    /// Multiplies the point by the scalar.
    /// </summary>
    /// <param name="a">Point.</param>
    /// <param name="b">Scalar.</param>
    /// <returns>Resulting point.</returns>
    public static Point operator *(float b, Point a) => new(a.X * b, a.Y * b);
    /// <summary>
    /// Divides the point by the scalar.
    /// </summary>
    /// <param name="a">Point.</param>
    /// <param name="b">Scalar.</param>
    /// <returns>Resulting point.</returns>
    public static Point operator /(Point a, float b) => new(a.X / b, a.Y / b);
}