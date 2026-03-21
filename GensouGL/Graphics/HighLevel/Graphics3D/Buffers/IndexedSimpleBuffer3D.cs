using System.Reflection.Metadata.Ecma335;

namespace GensouGL.Graphics;

internal sealed class IndexedSimpleBuffer3D : IDisposable
{
    private int _ebo;
    private int _vao;
    private int _vbuf;
    private int _cbuf;
    private int _nbuf;
    private int _tbuf;

    private bool _disposed = false;

    public IndexedSimpleBuffer3D()
    {
        _vao = GL.GenVertexArray();
        _ebo = GL.GenBuffer();
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

    public void SetData(Span<float> va, Span<float> ca, Span<float> na, Span<float> ta)
    {
        if (va.Length == 0) return;

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Length, ref va[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ca.Length, ref ca[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _nbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * na.Length, ref na[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ta.Length, ref ta[0], BufferUsageHint.DynamicDraw);


        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cbuf);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _nbuf);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    public void SetData(Span<float> va, Span<float> ca, Span<float> na)
    {
        if (va.Length == 0) return;

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

        GL.BindVertexArray(0);
    }

    public void SetIndices(Span<int> indices)
    {
        if (indices.Length == 0) return;

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, ref indices[0], BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public void DrawCall(PrimitiveType type, int vertexCount)
    {
        GL.BindVertexArray(_vao);
        GL.DisableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        GL.DrawElements(type, vertexCount, DrawElementsType.UnsignedInt, 0);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void DrawCallWithTexture(PrimitiveType type, int vertexCount)
    {
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        GL.DrawElements(type, vertexCount, DrawElementsType.UnsignedInt, 0);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Draw(PrimitiveType type, Span<float> va, Span<float> ca, Span<float> na)
    {
        SetData(va, ca, na);

        DrawCall(type, va.Length / 3);
    }

    public void Draw(PrimitiveType type, Span<float> va, Span<float> ca, Span<float> na, Span<float> ta)
    {
        SetData(va, ca, na, ta);

        DrawCallWithTexture(type, va.Length / 3);
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