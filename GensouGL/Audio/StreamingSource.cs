namespace GensouGL.Core;

public abstract class StreamingSource : IDisposable
{
    protected bool _disposed = false;

    protected abstract void Dispose(bool disposing);

    public abstract int Read(byte[] buffer, int length);
    public abstract void Seek(long position);
    public abstract void GetInfo(out int sampleRate, out int bitsPerSample, out int channels, out float duration);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~StreamingSource()
    {
        Dispose(false);
    }
}