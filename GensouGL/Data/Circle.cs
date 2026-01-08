namespace GensouGL.Core;

/// <summary>
/// Represents a circle.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Circle
{
    /// <summary>
    /// X coordinate of the center.
    /// </summary>
    public float X;
    /// <summary>
    /// Y coordinate of the center.
    /// </summary>
    public float Y;
    /// <summary>
    /// Radius.
    /// </summary>
    public float Radius;

    /// <summary>
    /// Initializes <see cref="Circle"/> with the specified parameters.
    /// </summary>
    /// <param name="x">X coordinate of the center.</param>
    /// <param name="y">Y coordinate of the center.</param>
    /// <param name="radius">Radius.</param>
    public Circle(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    /// <summary>
    /// Gets the center point.
    /// </summary>
    public readonly Point Point => new(X, Y);
    /// <summary>
    /// Gets the bounding AABB.
    /// </summary>
    public readonly Rect Bounds => new(X - Radius, Y - Radius, Radius * 2, Radius * 2);

    /// <summary>
    /// Returns a circle with the center point (X + offsetX, Y + offsetY).
    /// </summary>
    /// <param name="offsetX">X offset.</param>
    /// <param name="offsetY">Y offset.</param>
    /// <returns>Circle with the translated center point.</returns>
    public readonly Circle Offset(float offsetX, float offsetY)
    {
        return new Circle(X + offsetX, Y + offsetY, Radius);
    }

    /// <summary>
    /// Returns a circle with the specified radius and current center point.
    /// </summary>
    /// <param name="newRadius">New radius.</param>
    /// <returns>Circle with the new radius.</returns>
    public readonly Circle Resize(float newRadius)
    {
        return new Circle(X, Y, newRadius);
    }

    /// <summary>
    /// Determines if the specified point is contained in or on the circumference of the circle.
    /// </summary>
    /// <param name="point">Target point.</param>
    /// <returns>Flag representing whether the point is contained in the circle.</returns>
    public readonly bool Contains(Point point)
    {
        float dx = point.X - X;
        float dy = point.Y - Y;
        
        return (dx * dx + dy * dy) <= (Radius * Radius);
    }

    /// <summary>
    /// Determines if the intersection of the specified circle and the current circle has a positive area or is tangent. 
    /// </summary>
    /// <param name="other">Target circle.</param>
    /// <returns>Flag representing whether the specified circle is intersecting with the current circle.</returns>
    public readonly bool Intersects(Circle other)
    {
        float dx = other.X - X;
        float dy = other.Y - Y;
        float radiiSum = Radius + other.Radius;
        return (dx * dx + dy * dy) <= (radiiSum * radiiSum);
    }

    /// <summary>
    /// Determines if the specified rectangle is contained in the circle.
    /// </summary>
    /// <param name="rect">Target rectangle.</param>
    /// <returns>Flag representing whether the rectangle is contained in the circle.</returns>
    public readonly bool Contains(Rect rect)
    {
        return Contains(new Point(rect.X, rect.Y)) &&
               Contains(new Point(rect.X + rect.Width, rect.Y)) &&
               Contains(new Point(rect.X, rect.Y + rect.Height)) &&
               Contains(new Point(rect.X + rect.Width, rect.Y + rect.Height));
    }
}