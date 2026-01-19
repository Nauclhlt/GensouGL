namespace GensouGL.Graphics;

/// <summary>
/// Represents an abstract base class for all encoder classes.
/// </summary>
public abstract class GraphicsEncoder3D
{
    /// <summary>
    /// Current material.
    /// </summary>
    protected Material _material;
    /// <summary>
    /// Current context.
    /// </summary>
    protected RenderContext3D _context;

    /// <summary>
    /// Gets or sets the current material.
    /// </summary>
    public Material Material
    {
        get => _material;
        set
        {
            if (value is null) return;
            _material = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="GraphicsEncoder3D"/> using the specified context.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public GraphicsEncoder3D(RenderContext3D context)
    {
        _context = context;
    }

    internal void Accumulate(IDrawCommand3D command)
    {
        _context.Accumulate(command);
    }
}