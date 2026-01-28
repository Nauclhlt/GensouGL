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

    /// <summary>
    /// Encodes the rectangle parallel to the XY-plane.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="z">Z coordinate.</param>
    /// <param name="dirPositiveZ">Whether the rectangle faces the positive z-axis.</param>
    /// <param name="color">Color of the rectangle.</param>
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

        Accumulate(new SimpleShapeCommand(PrimitiveType.TriangleStrip, 
            _material, 
            _transform, 
            new RenderKey(color.A == 1f ? RenderPriority.Opaque : RenderPriority.Transparent, new Vec3(rect.CenterPoint.X, rect.CenterPoint.Y, z)),
            va, ca, na));
    }

    /// <summary>
    /// Encodes the rectangle parallel to the XZ-plane.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="dirPositiveY">Whether the rectangle faces the positive y-axis.</param>
    /// <param name="color">Color of the rectangle.</param>
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

        Accumulate(new SimpleShapeCommand(PrimitiveType.TriangleStrip, 
            _material, 
            _transform, 
            new RenderKey(color.A == 1f ? RenderPriority.Opaque : RenderPriority.Transparent, new Vec3(rect.CenterPoint.X, y, rect.CenterPoint.Y)),
            va, ca, na));
    }

    /// <summary>
    /// Encodes the rectangle parallel to the YZ-plane.
    /// </summary>
    /// <param name="rect">Rectangle (X mapping to Y, Y mapping to Z).</param>
    /// <param name="x">X coordinate (position on the X-axis).</param>
    /// <param name="dirPositiveX">Whether the rectangle faces the positive x-axis.</param>
    /// <param name="color">Color of the rectangle.</param>
    public void EncodeRectYZ(Rect rect, float x, bool dirPositiveX, Color color)
    {
        float[] va, ca, na;
        if (dirPositiveX)
        {
            va = [
                x, rect.X, rect.Y + rect.Height,
                x, rect.X, rect.Y,
                x, rect.X + rect.Width, rect.Y + rect.Height,
                x, rect.X + rect.Width, rect.Y,
            ];

            na = [
                1f, 0, 0,
                1f, 0, 0,
                1f, 0, 0,
                1f, 0, 0,
            ];
        }
        else
        {
            va = [
                x, rect.X, rect.Y + rect.Height,
                x, rect.X + rect.Width, rect.Y + rect.Height,
                x, rect.X, rect.Y,
                x, rect.X + rect.Width, rect.Y,
            ];

            na = [
                -1f, 0, 0,
                -1f, 0, 0,
                -1f, 0, 0,
                -1f, 0, 0,
            ];
        }

        ca = [
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
            color.R, color.G, color.B, color.A,
        ];

        Accumulate(new SimpleShapeCommand(PrimitiveType.TriangleStrip, 
            _material, 
            _transform, 
            new RenderKey(color.A == 1f ? RenderPriority.Opaque : RenderPriority.Transparent, new Vec3(x, rect.CenterPoint.X, rect.CenterPoint.Y)),
            va, ca, na));
    }

    /// <summary>
    /// Encodes the 3D cube.
    /// </summary>
    /// <param name="center">Center point of the cube.</param>
    /// <param name="size">Size of the cube.</param>
    /// <param name="color">Color of the cube.</param>
    public void EncodeCube(Vec3 center, Vec3 size, Color color)
    {
        EncodeCube(center.X, center.Y, center.Z, size.X, size.Y, size.Z, color);
    }

    /// <summary>
    /// Encodes the 3D cube.
    /// </summary>
    /// <param name="center">Center point of the cube.</param>
    /// <param name="width">Width of the cube.</param>
    /// <param name="height">Height of the cube.</param>
    /// <param name="depth">Depth of the cube.</param>
    /// <param name="color">Color of the cube.</param>
    public void EncodeCube(Vec3 center, float width, float height, float depth, Color color)
    {
        EncodeCube(center.X, center.Y, center.Z, width, height, depth, color);
    }

    /// <summary>
    /// Encodes the 3D cube.
    /// </summary>
    /// <param name="centerX">X coordinate of the center point.</param>
    /// <param name="centerY">Y coordinate of the center point.</param>
    /// <param name="centerZ">Z coordinate of the center point.</param>
    /// <param name="width">Width of the cube.</param>
    /// <param name="height">Height of the cube.</param>
    /// <param name="depth">Depth of the cube.</param>
    /// <param name="color">Color of the cube.</param>
    public void EncodeCube(float centerX, float centerY, float centerZ, float width, float height, float depth, Color color)
    {
        const int vertexCount = 36;
        float[] va = new float[vertexCount * 3];
        float[] na = new float[vertexCount * 3];
        float[] ca = new float[vertexCount * 4];

        float x0 = centerX - width / 2f, x1 = centerX + width / 2f;
        float y0 = centerY - height / 2f, y1 = centerY + height / 2f;
        float z0 = centerZ - depth / 2f, z1 = centerZ + depth / 2f;

        int vIdx = 0;
        int cIdx = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteFace(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z,
            float nx, float ny, float nz)
        {
            WriteVertex(v0x, v0y, v0z, nx, ny, nz);
            WriteVertex(v1x, v1y, v1z, nx, ny, nz);
            WriteVertex(v2x, v2y, v2z, nx, ny, nz);
            WriteVertex(v2x, v2y, v2z, nx, ny, nz);
            WriteVertex(v1x, v1y, v1z, nx, ny, nz);
            WriteVertex(v3x, v3y, v3z, nx, ny, nz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteVertex(float vx, float vy, float vz, float nx, float ny, float nz)
        {
            va[vIdx] = vx; va[vIdx + 1] = vy; va[vIdx + 2] = vz;
            na[vIdx] = nx; na[vIdx + 1] = ny; na[vIdx + 2] = nz;
            vIdx += 3;

            ca[cIdx] = color.R; ca[cIdx + 1] = color.G;
            ca[cIdx + 2] = color.B; ca[cIdx + 3] = color.A;
            cIdx += 4;
        }

        WriteFace(x0, y1, z1, x0, y0, z1, x1, y1, z1, x1, y0, z1, 0, 0, 1);  // Front
        WriteFace(x1, y1, z0, x1, y0, z0, x0, y1, z0, x0, y0, z0, 0, 0, -1); // Back
        WriteFace(x1, y1, z1, x1, y0, z1, x1, y1, z0, x1, y0, z0, 1, 0, 0);  // Right
        WriteFace(x0, y1, z0, x0, y0, z0, x0, y1, z1, x0, y0, z1, -1, 0, 0); // Left
        WriteFace(x0, y1, z0, x0, y1, z1, x1, y1, z0, x1, y1, z1, 0, 1, 0);  // Top
        WriteFace(x0, y0, z1, x0, y0, z0, x1, y0, z1, x1, y0, z0, 0, -1, 0); // Bottom

        Accumulate(new SimpleShapeCommand(PrimitiveType.Triangles, 
            _material, 
            _transform, 
            new RenderKey(color.A == 1f ? RenderPriority.Opaque : RenderPriority.Transparent, new Vec3(centerX, centerY, centerZ)),
            va, ca, na));
    }

    /// <summary>
    /// Encodes the sphere.
    /// </summary>
    /// <param name="centerX">X coordinate of the center point.</param>
    /// <param name="centerY">Y coordinate of the center point.</param>
    /// <param name="centerZ">Z coordinate of the center point.</param>
    /// <param name="radius">Radius of the sphere.</param>
    /// <param name="color">Color of the sphere.</param>
    /// <param name="div">Division count.</param>
    public void EncodeSphere(float centerX, float centerY, float centerZ, float radius, Color color, int div = 32)
    {
        int quadCount = div * div;
        int vertexCount = quadCount * 6;

        float[] va = new float[vertexCount * 3];
        float[] na = new float[vertexCount * 3];
        float[] ca = new float[vertexCount * 4];

        int vIdx = 0;
        int cIdx = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteVertex(float vx, float vy, float vz)
        {
            va[vIdx] = vx;
            va[vIdx + 1] = vy;
            va[vIdx + 2] = vz;

            float nx = (vx - centerX) / radius;
            float ny = (vy - centerY) / radius;
            float nz = (vz - centerZ) / radius;

            na[vIdx] = nx;
            na[vIdx + 1] = ny;
            na[vIdx + 2] = nz;

            vIdx += 3;

            ca[cIdx] = color.R;
            ca[cIdx + 1] = color.G;
            ca[cIdx + 2] = color.B;
            ca[cIdx + 3] = color.A;
            cIdx += 4;
        }

        for (int y = 0; y < div; y++)
        {
            float v0 = (float)y / div;
            float v1 = (float)(y + 1) / div;

            float theta0 = v0 * MathF.PI;
            float theta1 = v1 * MathF.PI;

            float sin0 = MathF.Sin(theta0);
            float cos0 = MathF.Cos(theta0);
            float sin1 = MathF.Sin(theta1);
            float cos1 = MathF.Cos(theta1);

            for (int x = 0; x < div; x++)
            {
                float u0 = (float)x / div;
                float u1 = (float)(x + 1) / div;

                float phi0 = u0 * MathF.Tau;
                float phi1 = u1 * MathF.Tau;

                float sp0 = MathF.Sin(phi0);
                float cp0 = MathF.Cos(phi0);
                float sp1 = MathF.Sin(phi1);
                float cp1 = MathF.Cos(phi1);

                float x00 = centerX + radius * sin0 * cp0;
                float y00 = centerY + radius * cos0;
                float z00 = centerZ + radius * sin0 * sp0;

                float x10 = centerX + radius * sin0 * cp1;
                float y10 = centerY + radius * cos0;
                float z10 = centerZ + radius * sin0 * sp1;

                float x01 = centerX + radius * sin1 * cp0;
                float y01 = centerY + radius * cos1;
                float z01 = centerZ + radius * sin1 * sp0;

                float x11 = centerX + radius * sin1 * cp1;
                float y11 = centerY + radius * cos1;
                float z11 = centerZ + radius * sin1 * sp1;

                WriteVertex(x00, y00, z00);
                WriteVertex(x01, y01, z01);
                WriteVertex(x10, y10, z10);

                WriteVertex(x10, y10, z10);
                WriteVertex(x01, y01, z01);
                WriteVertex(x11, y11, z11);
            }
        }
        
        Accumulate(new SimpleShapeCommand(PrimitiveType.Triangles, 
            _material, 
            _transform, 
            new RenderKey(color.A == 1f ? RenderPriority.Opaque : RenderPriority.Transparent, new Vec3(centerX, centerY, centerZ)),
            va, ca, na));
    }
}