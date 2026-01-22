namespace GensouGL.Graphics;

/// <summary>
/// Specifies the types of geometric primitives that can be used when rendering 3D graphics primitives, such as points,
/// lines, triangles, and patches.
/// </summary>
/// <remarks>This enumeration corresponds to the primitive types defined by the OpenGL API and is typically used
/// to indicate how vertex data should be interpreted during rendering. Some values may require specific OpenGL versions
/// or extensions to be supported. Duplicate values exist to provide compatibility with different OpenGL profiles and
/// extensions.</remarks>
public enum PrimitiveType3D
{
    /// <summary>
    /// Points.
    /// </summary>
    Points = 0,
    /// <summary>
    /// Lines.
    /// </summary>
    Lines = 1,
    /// <summary>
    /// LineLoop.
    /// </summary>
    LineLoop = 2,
    /// <summary>
    /// LineStrip.
    /// </summary>
    LineStrip = 3,
    /// <summary>
    /// Triangles.
    /// </summary>
    Triangles = 4,
    /// <summary>
    /// TriangleStrip.
    /// </summary>
    TriangleStrip = 5,
    /// <summary>
    /// TriangleFan.
    /// </summary>
    TriangleFan = 6,
    /// <summary>
    /// Quads.
    /// </summary>
    Quads = 7,
    /// <summary>
    /// LinesAdjacency.
    /// </summary>
    LinesAdjacency = 10,
    /// <summary>
    /// LineStripAdjancency.
    /// </summary>
    LineStripAdjacency = 11,
    /// <summary>
    /// TrianglesAdjacency.
    /// </summary>
    TrianglesAdjacency = 12,
    /// <summary>
    /// TriangleStripAdjacency.
    /// </summary>
    TriangleStripAdjacency = 13,
    /// <summary>
    /// Patches.
    /// </summary>
    Patches = 14,
}