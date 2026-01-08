namespace GensouGL.Core;

/// <summary>
/// Represents a rectangular gradient with four corner colors.
/// </summary>
public struct RectGradient
{
    /// <summary>
    /// Top left color.
    /// </summary>
    public Color TopLeft;
    /// <summary>
    /// Bottom left color.
    /// </summary>
    public Color BottomLeft;
    /// <summary>
    /// Bottom right color.
    /// </summary>
    public Color BottomRight;
    /// <summary>
    /// Top right color.
    /// </summary>
    public Color TopRight;
    

    /// <summary>
    /// Initializes <see cref="RectGradient" /> using the specified parameters.
    /// </summary>
    /// <param name="topLeft">Top left color.</param>
    /// <param name="bottomLeft">Bottom left color.</param>
    /// <param name="bottomRight">Bottom right color.</param>
    /// <param name="topRight">Top right color.</param>
    public RectGradient(Color topLeft, Color bottomLeft, Color bottomRight, Color topRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
    }

    /// <summary>
    /// Initializes single colored RectGradient using the specified color.
    /// </summary>
    /// <param name="color">Color.</param>
    public RectGradient(Color color)
    {
        TopLeft = color;
        TopRight = color;
        BottomLeft = color;
        BottomRight = color;
    }

    /// <summary>
    /// Returns RectGradient with a vertical gradient of top and bottom colors.
    /// </summary>
    /// <param name="top">Top color.</param>
    /// <param name="bottom">Bottom color.</param>
    /// <returns>Vertical RectGradient.</returns>
    public static RectGradient Vertical(Color top, Color bottom)
    {
        return new RectGradient(top, bottom, bottom, top);
    }

    /// <summary>
    /// Returns RectGradient with a horizontal gradient of left and right color.
    /// </summary>
    /// <param name="left">Left color.</param>
    /// <param name="right">Right color.</param>
    /// <returns>Horizontal RectGradient.</returns>
    public static RectGradient Horizontal(Color left, Color right)
    {
        return new RectGradient(left, left, right, right);
    }
}