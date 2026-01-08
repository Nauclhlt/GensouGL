namespace GensouGL.Graphics;

/// <summary>
/// Represents a high-performance bullet renderer that batches draw calls to minimize GPU state changes.
/// Bullet is: mass-generated objects defined by (center) position, size, texture, color, and rotation angle.
/// </summary>
public sealed class BatchBulletRenderer2D : Renderer2D
{
    private static BatchBulletRendererBuffer _buffer;

    internal static void Initialize()
    {
        _buffer = new(Vertices, Indices);
    }

    internal static void Release()
    {
        _buffer.Dispose();
    }


    private static readonly float[] Vertices =
    {
        -0.5f, -0.5f,
        -0.5f, 0.5f,
        0.5f, -0.5f,
        0.5f, 0.5f
    };

    private static readonly int[] Indices =
    {
        0, 1, 2, 2, 1, 3
    };


    private ITexture _globalTexture = null;
    private List<float> _colors = new(256);
    private List<float> _texCoords = new(256);
    private List<float> _positions = new(256);
    private List<float> _rotations = new(256);
    private List<float> _scales = new(256);


    private BatchBulletRenderer2D()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="BatchBulletRenderer2D"/> with the specified screen.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public BatchBulletRenderer2D(Screen screen)
    {
        Setup(screen);
    }

    /// <summary>
    /// Begins a drawing process using the specified global texture.
    /// </summary>
    /// <param name="globalTexture">Global texture.</param>
    public void Begin(ITexture globalTexture)
    {
        if (_globalTexture is not null)
        {
            DebugLog.Warn($"{nameof(BatchBulletRenderer2D)}: Begin() called before End().");
        }

        _globalTexture = globalTexture;
        _colors.Clear();
        _texCoords.Clear();
        _positions.Clear();
        _rotations.Clear();
        _scales.Clear();
    }

    /// <summary>
    /// Draws one bullet using the specified parameters.
    /// </summary>
    /// <param name="x">X coordinate of the bullet.</param>
    /// <param name="y">Y coordinate of the bullet.</param>
    /// <param name="width">Width of the bullet.</param>
    /// <param name="height">Height of the bullet.</param>
    /// <param name="color">Color of the bullet.</param>
    /// <param name="texture">Texture of the bullet.</param>
    public void Draw(float x, float y, float width, float height, Color color, ITexture texture)
    {
        Draw(x, y, width, height, 0f, color, texture);
    }

    /// <summary>
    /// Draws one bullet using the specified parameters.
    /// </summary>
    /// <param name="center">Position of the bullet.</param>
    /// <param name="width">Width of the bullet.</param>
    /// <param name="height">Height of the bullet.</param>
    /// <param name="color">Color of the bullet.</param>
    /// <param name="texture">Texture of the bullet.</param>
    public void Draw(Point center, float width, float height, float rotation, Color color, ITexture texture)
    {
        Draw(center.X, center.Y, width, height, rotation, color, texture);
    }

    /// <summary>
    /// Draws one bullet using the specified parameters.
    /// </summary>
    /// <param name="cx">X coordinate of the bullet.</param>
    /// <param name="cy">Y coordinate of the bullet.</param>
    /// <param name="width">Width of the bullet.</param>
    /// <param name="height">Height of the bullet.</param>
    /// <param name="rotation">Rotation angle of the bullet.</param>
    /// <param name="color">Color of the bullet.</param>
    /// <param name="texture">Texture of the bullet.</param>
    public void Draw(float cx, float cy, float width, float height, float rotation, Color color, ITexture texture)
    {
        if (_globalTexture is null)
        {
            DebugLog.Error($"{nameof(BatchBulletRenderer2D)}: Draw() called before Begin().");
            return;
        }

        if (_globalTexture.TextureId != texture.TextureId)
        {
            DebugLog.Error($"{nameof(BatchBulletRenderer2D)}: Inconsistent texture IDs between the global texture and the given texture.");
            return;
        }

        _colors.Add(color.R);
        _colors.Add(color.G);
        _colors.Add(color.B);
        _colors.Add(color.A);

        _texCoords.Add(texture.Coords.X);
        _texCoords.Add(texture.Coords.Y);

        _texCoords.Add(texture.Coords.X);
        _texCoords.Add(texture.Coords.Bottom);

        _texCoords.Add(texture.Coords.Right);
        _texCoords.Add(texture.Coords.Y);

        _texCoords.Add(texture.Coords.Right);
        _texCoords.Add(texture.Coords.Bottom);

        _positions.Add(cx);
        _positions.Add(cy);

        _rotations.Add(rotation);

        _scales.Add(width);
        _scales.Add(height);
    }

    /// <summary>
    /// Finalizes the batch and submits all accumulated data to the GPU.
    /// </summary>
    public void End()
    {
        if (_globalTexture is null)
        {
            DebugLog.Error($"{nameof(BatchBulletRenderer2D)}: End() called before Begin().");
            return;
        }

        if (_colors.Count == 0)
        {
            _globalTexture = null;
            return;
        }

        LoadState();

        Shader.Bullet.Bind();
        Shader.SendMatrix0(GetDefaultVMatrix());

        TextureService.PushAndBind(_globalTexture.TextureId);

        _buffer.Draw(PrimitiveType.Triangles, _colors, _texCoords, _positions, _rotations, _scales);

        TextureService.Pop();

        RestoreState();
        _globalTexture = null;
    }
}

internal sealed class BatchBulletRendererBuffer : IDisposable
{
    private int _vao;

    private int _ebo;
    private int _vbuf;
    private int _cbuf;
    private int _tbuf;
    private int _pbuf;
    private int _rbuf;
    private int _sbuf;

    private bool _disposed = false;

    public BatchBulletRendererBuffer(float[] vertices, int[] indices)
    {
        _vao = GL.GenVertexArray();
        _ebo = GL.GenBuffer();
        _vbuf = GL.GenBuffer();
        _cbuf = GL.GenBuffer();
        _tbuf = GL.GenBuffer();
        _pbuf = GL.GenBuffer();
        _rbuf = GL.GenBuffer();
        _sbuf = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
        GL.EnableVertexAttribArray(4);
        GL.EnableVertexAttribArray(5);
        


        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, List<float> colors, List<float> texCoords, List<float> positions, List<float> rotations, List<float> scales)
    {
        Span<float> colorSpan = CollectionsMarshal.AsSpan(colors);
        Span<float> texCoordSpan = CollectionsMarshal.AsSpan(texCoords);
        Span<float> positionSpan = CollectionsMarshal.AsSpan(positions);
        Span<float> rotationSpan = CollectionsMarshal.AsSpan(rotations);
        Span<float> scaleSpan = CollectionsMarshal.AsSpan(scales);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * colorSpan.Length, ref colorSpan[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * texCoordSpan.Length, ref texCoordSpan[0], BufferUsageHint.DynamicDraw);


        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
        GL.VertexAttribDivisor(1, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _pbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * positionSpan.Length, ref positionSpan[0], BufferUsageHint.DynamicDraw);

        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.VertexAttribDivisor(3, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _rbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * rotationSpan.Length, ref rotationSpan[0], BufferUsageHint.DynamicDraw);

        GL.VertexAttribPointer(4, 1, VertexAttribPointerType.Float, false, 0, 0);
        GL.VertexAttribDivisor(4, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _sbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * scaleSpan.Length, ref scaleSpan[0], BufferUsageHint.DynamicDraw);

        GL.VertexAttribPointer(5, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.VertexAttribDivisor(5, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0, colorSpan.Length / 4);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbuf);
            GL.DeleteBuffer(_cbuf);
            GL.DeleteBuffer(_tbuf);

            _disposed = true;
        }
    }
}