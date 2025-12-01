namespace GensouGL.IO;

public sealed class PrimitiveReader : IDisposable
{
    private readonly BinaryReader _reader;
    public PrimitiveReader(Stream stream)
    {
        _reader = new BinaryReader(stream, Encoding.UTF8);
    }
    public PrimitiveReader(string filePath)
    {
        _reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read), Encoding.UTF8);
    }
    
    public bool TryReadByte(out byte value)
    {
        try
        {
            value = _reader.ReadByte();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadShort(out short value)
    {
        try
        {
            value = _reader.ReadInt16();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadInt(out int value)
    {
        try
        {
            value = _reader.ReadInt32();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadLong(out long value)
    {
        try
        {
            value = _reader.ReadInt64();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadFloat(out float value)
    {
        try
        {
            value = _reader.ReadSingle();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadDouble(out double value)
    {
        try
        {
            value = _reader.ReadDouble();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public bool TryReadString(out string value)
    {
        try
        {
            value = _reader.ReadString();
            string hash = _reader.ReadString();
            return HashHelper.ComputeSha256Hash(value) == hash;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposing)
            _reader.Dispose();
    }
}