namespace GensouGL.Graphics;

/// <summary>
/// Represents an encoder that encodes buffers.
/// </summary>
public sealed class BufferEncoder3D : GraphicsEncoder3D
{
    /// <summary>
    /// Initializes a new instance of <see cref="BufferEncoder3D"/> using the specified context.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public BufferEncoder3D(RenderContext3D context) : base(context)
    {
    }

    /// <summary>
    /// Encodes the specified buffer for rendering.
    /// </summary>
    /// <param name="buffer">Buffer to encode.</param>
    /// <param name="primitiveType">Type of primitives to render.</param>
    public void Encode(Buffer3D buffer, PrimitiveType3D primitiveType)
    {
        if (buffer is null) return;
        if (!buffer.Buffered) return;

        Accumulate(new Buffer3DCommand((PrimitiveType)primitiveType, _material, _transform, buffer));
    }

    /// <summary>
    /// Encodes the specified buffer for rendering.
    /// </summary>
    /// <param name="buffer">Buffer to encode.</param>
    /// <param name="primitiveType">Type of primitives to render.</param>
    /// <param name="texture">Texture to apply.</param>
    public void Encode(Buffer3D buffer, PrimitiveType3D primitiveType, ITexture texture)
    {
        if (buffer is null) return;
        if (!buffer.Buffered) return;
        if (!buffer.Textured) return;

        Accumulate(new Buffer3DCommand((PrimitiveType)primitiveType, _material, _transform, buffer, texture));
    }
}