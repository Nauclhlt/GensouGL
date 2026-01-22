namespace GensouGL.Graphics;

/// <summary>
/// Represents an encoder that creates simple shapes.
/// </summary>
public sealed class ShapeEncoder3D : GraphicsEncoder3D
{
    /// <summary>
    /// Initializes a new instance of <see cref="ShapeEncoder3D"/> with the specified context.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public ShapeEncoder3D(RenderContext3D context) : base(context)
    {
    }

    public void EncodeRectXY(Rect rect, float z, bool dirPositiveZ, Color color)
    {
        float[] va, ca, na;
        if (dirPositiveZ)
        {
            va = [
                rect.X, rect.Y + rect.Height, z,
                rect.X, rect.Y, z,
                rect.X + rect.Width, rect.Y + rect.Height, z,
                rect.X + rect.Width, rect.Y, z,
            ];

            na = [
                0, 0, 1f,
                0, 0, 1f,
                0, 0, 1f,
                0, 0, 1f,
            ];
        }
        else
        {
            va = [
                rect.X, rect.Y + rect.Height, z,
                rect.X + rect.Width, rect.Y + rect.Height, z,
                rect.X, rect.Y, z,
                rect.X + rect.Width, rect.Y + rect.Height, z,
            ];

            na = [
                0, 0, -1f,
                0, 0, -1f,
                0, 0, -1f,
                0, 0, -1f,
            ];
        }

        ca = [
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
        ];

        Accumulate(new SimpleShapeCommand(PrimitiveType.TriangleStrip, _material, va, ca, na));
    }

    public void EncodeRectXZ(Rect rect, float y, bool dirPositiveY, Color color)
    {
        float[] va, ca, na;

        if (dirPositiveY)
        {
            va = [
                rect.X, y, rect.Y,
                rect.X, y, rect.Y + rect.Height,
                rect.X + rect.Width, y, rect.Y,
                rect.X + rect.Width, y, rect.Y + rect.Height,
            ];

            na = [
                0, 1f, 0,
                0, 1f, 0,
                0, 1f, 0,
                0, 1f, 0,
            ];
        }
        else
        {
            va = [
                rect.X, y, rect.Y,
                rect.Right, y, rect.Y,
                rect.X, y, rect.Bottom,
                rect.Right, y, rect.Bottom,
            ];

            na = [
                0, -1f, 0,
                0, -1f, 0,
                0, -1f, 0,
                0, -1f, 0,
            ];
        }


        ca = [
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
        ];

        Accumulate(new SimpleShapeCommand(PrimitiveType.TriangleStrip, _material, va, ca, na));
    }
}