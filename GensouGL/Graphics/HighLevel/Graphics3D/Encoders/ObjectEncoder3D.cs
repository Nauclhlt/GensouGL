namespace GensouGL.Graphics;

/// <summary>
/// Provides functionality to encode various objects for rendering using a specified rendering context.
/// </summary>
public sealed class ObjectEncoder3D : GraphicsEncoder3D
{
    /// <summary>
    /// Initializes a new instance of <see cref="ObjectEncoder3D"/> using the specified context.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public ObjectEncoder3D(RenderContext3D context) : base(context)
    {
    }
}