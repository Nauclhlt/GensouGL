namespace GensouGL.Graphics;

internal sealed class SimpleBuffer3D : IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _nbuf;
    private int _tbuf;

    private bool _disposed = false;

    public SimpleBuffer3D()
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