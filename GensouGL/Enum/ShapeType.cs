namespace GensouGL.Core;

/// <summary>
/// Specifies the type of shape.
/// </summary>
public enum ShapeType
{
    /// <summary>
    /// Invalid shape type.
    /// </summary>
    /// <remarks>
    /// Do not use this item. 
    /// </remarks>
    Invalid,
    /// <summary>
    /// Rectangle shape type.
    /// </summary>
    Rect,
    /// <summary>
    /// Wire rectangle shape type.
    /// </summary>
    WireRect,
    /// <summary>
    /// Line shape type.
    /// </summary>
    Line,
    /// <summary>
    /// Triangle shape type.
    /// </summary>
    Triangle,
    /// <summary>
    /// Wire triangle shape type.
    /// </summary>
    WireTriangle,
    /// <summary>
    /// Ellipse shape type.
    /// </summary>
    Ellipse
}

internal static class ShapeTypeExtensions
{
    internal static PrimitiveType ToPrimitiveType(this ShapeType self)
    {
        return self switch
        {
            ShapeType.Rect => PrimitiveType.Triangles,
            ShapeType.WireRect => PrimitiveType.Lines,
            ShapeType.Line => PrimitiveType.Lines,
            ShapeType.Triangle => PrimitiveType.Triangles,
            ShapeType.WireTriangle => PrimitiveType.Lines,
            ShapeType.Ellipse => PrimitiveType.Triangles,
            _ => PrimitiveType.Triangles
        };
    }
}