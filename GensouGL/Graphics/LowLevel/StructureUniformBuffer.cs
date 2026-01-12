namespace GensouGL.Graphics;

internal sealed class StructureUniformBuffer<T> : IDisposable where T : struct
{
    private int _buffer;
    private int _size;
    private bool _disposed = false;

    public StructureUniformBuffer()
    {
        _size = Marshal.SizeOf<T>();

        _buffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, _buffer);

        GL.BufferData(BufferTarget.UniformBuffer, _size, IntPtr.Zero, BufferUsageHint.DynamicDraw);

        //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
    }

    public void Update(T data)
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _buffer);

        GL.BufferSubData(BufferTarget.UniformBuffer, 0, _size, ref data);

        //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
    }

    public byte[] GetData()
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _buffer);

        byte[] bytes = new byte[_size];
        GL.GetBufferSubData(BufferTarget.UniformBuffer, 0, _size, bytes);

        return bytes;
    }

    public void Bind(int index)
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _buffer);
        GL.BindBufferBase(BufferRangeTarget.UniformBuffer, index, _buffer);
        //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
    }

    public void Unbind(int index)
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _buffer);
        GL.BindBufferBase(BufferRangeTarget.UniformBuffer, index, 0);
        //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
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

    ~StructureUniformBuffer()
    {
        Dispose(false);
    }
}