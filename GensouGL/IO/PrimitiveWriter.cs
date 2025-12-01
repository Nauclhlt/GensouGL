namespace GensouGL.IO;

public class PrimitiveWriter : IDisposable
{
    private readonly BinaryWriter _writer;

    public PrimitiveWriter(Stream stream)
    {
        _writer = new BinaryWriter(stream, Encoding.UTF8);
    }

    public PrimitiveWriter(string filePath)
    {
        _writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write), Encoding.UTF8);
    }

    public void Write(byte value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(short value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(int value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(long value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(float value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(double value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    public void Write(string value)
    {
        _writer.Write(value);
        _writer.Write(HashHelper.ComputeSha256Hash(value));
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            _writer?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~PrimitiveWriter()
    {
        Dispose(false);
    }
}