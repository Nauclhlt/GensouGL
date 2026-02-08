namespace GensouGL.Core;

/// <summary>
/// Represents AABB using top left coordinate and size.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    /// <summary>
    /// X coordinate.
    /// </summary>
    public float X;
    /// <summary>
    /// Y coordinate.
    /// </summary>
    public float Y;
    /// <summary>
    /// Width.
    /// </summary>
    public float Width;
    /// <summary>
    /// Height.
    /// </summary>
    public float Height;

    /// <summary>
    /// Gets the top left point.
    /// </summary>
    public readonly Point Point => new(X, Y);
    
    /// <summary>
    /// Initializes <see cref="Rect"/> using the specified parameters.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public Rect(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the area of the rectangle.
    /// </summary>
    public readonly float Area => Width * Height;
    /// <summary>
    /// Gets the X coordinate of the left side.
    /// </summary>
    public readonly float Left => X;
    /// <summary>
    /// Gets the Y coordinate of the top side.
    /// </summary>
    public readonly float Top => Y;
    /// <summary>
    /// Gets the X coordinate of the right side.
    /// </summary>
    public readonly float Right => X + Width;
    /// <summary>
    /// Gets the Y coordinate of the bottom side.
    /// </summary>
    public readonly float Bottom => Y + Height;
    /// <summary>
    /// Gets the center point of the rectangle.
    /// </summary>
    public readonly Point CenterPoint => new(X + Width / 2, Y + Height / 2);
    /// <summary>
    /// Gets the top left point of the rectangle.
    /// </summary>
    public readonly Point TopLeft => new(Left, Top);
    /// <summary>
    /// Gets the top right point of the rectangle.
    /// </summary>
    public readonly Point TopRight => new(Right, Top);
    /// <summary>
    /// Gets the bottom left point of the rectangle.
    /// </summary>
    public readonly Point BottomLeft => new(Left, Bottom);
    /// <summary>
    /// Gets the bottom right point of the rectangle.
    /// </summary>
    public readonly Point BottomRight => new(Right, Bottom);
    /// <summary>
    /// Gets whether the rectangle has positive area.
    /// </summary>
    public readonly bool HasArea => Width != 0 && Height != 0;


    /// <summary>
    /// Returns a rectangle with the top left point (0, 0) and size (0, 0).
    /// </summary>
    public static Rect Zero => new(0, 0, 0, 0);

    /// <summary>
    /// Returns a rectangle whose top left point is (X + offsetX, Y + offsetY).
    /// </summary>
    /// <param name="offsetX">X offset.</param>
    /// <param name="offsetY">Y offset.</param>
    /// <returns>Translated rectangle.</returns>
    public readonly Rect Offset(float offsetX, float offsetY)
    {
        return new Rect(X + offsetX, Y + offsetY, Width, Height);
    }

    /// <summary>
    /// Returns a resized rectangle.
    /// </summary>
    /// <param name="newWidth">New width.</param>
    /// <param name="newHeight">New height.</param>
    /// <returns>Resized rectangle.</returns>
    public readonly Rect Resize(float newWidth, float newHeight)
    {
        return new Rect(X, Y, newWidth, newHeight);
    }

    /// <summary>
    /// Returns a rectangle with the specified dimensions centered on the current rectangle.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <returns>Centered rectangle.</returns>
    public readonly Rect Center(float width, float height)
    {
        return new Rect(X + (Width - width) / 2f, Y + (Height - height) / 2f, width, height);
    }

    /// <summary>
    /// Returns an actual rectangle from a rectangle in a coordinate system normalized to the current rectangle.
    /// </summary>
    /// <param name="rect">Source rectangle.</param>
    /// <returns>Partial rectangle.</returns>
    public readonly Rect GetPartialRelative(Rect rect)
    {
        return new Rect(
            X + rect.X * Width,
            Y + rect.Y * Height,
            rect.Width * Width,
            rect.Height * Height
        );
    }

    /// <summary>
    /// Determines if the specified point is contained in the rectangle.
    /// </summary>
    /// <param name="point">Target point.</param>
    /// <returns>Flag representing whether the point is contained in the rectangle.</returns>
    public readonly bool Contains(Point point)
    {
        return point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
    }

    /// <summary>
    /// Determines if the specified rectangle is contained in the current rectangle.
    /// </summary>
    /// <param name="rect">Target rectangle.</param>
    /// <returns>Flag representing whether the specified rectangle is contained in the current rectangle.</returns>
    public readonly bool Contains(Rect rect)
    {
        return rect.Left >= Left && rect.Right <= Right && rect.Top >= Top && rect.Bottom <= Bottom;
    }

    /// <summary>
    /// Determintes whether the specified circle is contained in the rectangle.
    /// </summary>
    /// <param name="circle">Target circle.</param>
    /// <returns>Flag representing whether the specified circle is contained in the rectangle.</returns>
    public readonly bool Contains(Circle circle)
    {
        return circle.X - circle.Radius >= Left &&
               circle.X + circle.Radius <= Right &&
               circle.Y - circle.Radius >= Top &&
               circle.Y + circle.Radius <= Bottom;
    }

    /// <summary>
    /// Determines whether the area of ​​the intersection of the specified rectangle and the current rectangle is positive.
    /// </summary>
    /// <param name="rect">Target rectangle.</param>
    /// <returns>Flag representing the specified rectangle is intersecting with the current rectangle.</returns>
    public readonly bool Intersects(Rect rect)
    {
        return !(rect.Left > Right || rect.Right < Left || rect.Top > Bottom || rect.Bottom < Top);
    }

    /// <summary>
    /// Determines whether the area of ​​the intersection of the specified circle and the current rectangle is positive.
    /// </summary>
    /// <param name="circle">Target circle</param>
    /// <returns>Flag representing the specified circle is intersecting with the current rectangle.</returns>
    public readonly bool Intersects(Circle circle)
    {
        float nearestX = MathF.Max(Left, MathF.Min(circle.X, Right));
        float nearestY = MathF.Max(Top, MathF.Min(circle.Y, Bottom));
        float deltaX = circle.X - nearestX;
        float deltaY = circle.Y - nearestY;
        return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
    }
}