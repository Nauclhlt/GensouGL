namespace GensouGL.Graphics;

internal sealed class ArrayShaderStorageBuffer<T> : IDisposable where T : struct
{
    private int _buffer;
    private int _size;
    private bool _disposed;

    public ArrayShaderStorageBuffer()
    {
        _buffer = GL.GenBuffer();
        _size = Marshal.SizeOf<T>();
    }

    public void Update(T[] data)
    {
        Update(data.AsSpan());
    }

    public void Update(Span<T> data)
    {
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _buffer);

        if (data.Length > 0)
            GL.BufferData(BufferTarget.ShaderStorageBuffer, _size * data.Length, ref data[0], BufferUsageHint.DynamicDraw);
        else
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, 0, BufferUsageHint.DynamicDraw);
        
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
    }

    public void Bind(int index)
    {
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, index, _buffer);
    }

    public void Unbind(int index)
    {
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, index, 0);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            if (_buffer != 0)
                GL.DeleteBuffer(_buffer);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ArrayShaderStorageBuffer()
    {
        Dispose(false);
    }
}