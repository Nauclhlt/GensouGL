namespace GensouGL.Graphics;

/// <summary>
/// Provides quick rendering features, which generates one draw call per object.
/// </summary>
public sealed class QuickRenderer3D : Renderer3D
{
    private static QuickRendererBuffer3D _buffer;

    internal static QuickRendererBuffer3D InternalBuffer => _buffer;

    internal static void Initialize()
    {
        _buffer = new();
    }

    internal static void Release()
    {
        _buffer.Dispose();
    }

    private QuickRenderer3D()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="QuickRenderer3D"/> with the specified screen.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public QuickRenderer3D(Screen screen)
    {
        Setup(screen);
    }

    private void BindShapeShader()
    {
        if (LightingEnabled) Shader.SimpleShape3DLighting.Bind();
        else Shader.SimpleShape3D.Bind();
    }

    public void DrawRectXY(Rect rect, float z, bool dirPositiveZ, Color color)
    {
        float[] va = null, ca = null, na = null;
        if (dirPositiveZ)
        {
            va = [
                rect.X, rect.Y, z,
                rect.X + rect.Width, rect.Y + rect.Height, z,
                rect.X, rect.Y + rect.Height, z,

                rect.X + rect.Width, rect.Y + rect.Height, z,
                rect.X, rect.Y, z,
                rect.X + rect.Width, rect.Y, z
            ];

            ca = [
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,

                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A
            ];

            na = [
                0, 0, 1f,
                0, 0, 1f,
                0, 0, 1f,

                0, 0, 1f,
                0, 0, 1f,
                0, 0, 1f,
            ];
        }
        else
        {
            va = [
                rect.X, rect.Y, z,
                rect.X, rect.Y + rect.Height, z,
                rect.X + rect.Width, rect.Y + rect.Height, z,

                rect.X + rect.Width, rect.Y + rect.Height, z,
                rect.X + rect.Width, rect.Y, z,
                rect.X, rect.Y, z
            ];

            ca = [
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,

                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A,
                color.R, color.G, color.B, color.A
            ];

            na = [
                0, 0, -1f,
                0, 0, -1f,
                0, 0, -1f,

                0, 0, -1f,
                0, 0, -1f,
                0, 0, -1f,
            ];
        }

        LoadState();
        PrepareLighting();

        BindShapeShader();

        SetDefaultMVP();

        _buffer.Draw(PrimitiveType.Triangles, va, ca, na);

        RestoreState();
        FinishLighting();
    }
}


internal sealed class QuickRendererBuffer3D : IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _nbuf;
    private int _tbuf;

    private bool _disposed = false;

    public QuickRendererBuffer3D()
    {
        _vao = GL.GenVertexArray();
        _vbuf = GL.GenBuffer();
        _cbuf = GL.GenBuffer();
        _nbuf = GL.GenBuffer();
        _tbuf = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, Span<float> va, Span<float> ca, Span<float> na)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Length, ref va[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ca.Length, ref ca[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _nbuf);

        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * na.Length, ref na[0], BufferUsageHint.DynamicDraw);


        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);

        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _nbuf);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.DrawArrays(type, 0, va.Length / 3);

        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbuf);
            GL.DeleteBuffer(_cbuf);
            GL.DeleteBuffer(_nbuf);
            GL.DeleteBuffer(_tbuf);

            _disposed = true;
        }
    }
}