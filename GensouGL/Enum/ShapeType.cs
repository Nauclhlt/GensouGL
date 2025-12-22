namespace GensouGL.Core;

public enum ShapeType
{
    Invalid,
    Rect,
    WireRect,
    Line,
    Triangle,
    WireTriangle,
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