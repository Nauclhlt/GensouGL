namespace GensouGL.Core;

internal sealed class MpegFileAdapter : StreamingSource
{
    private MpegFile _mpegFile;

    public MpegFileAdapter(string filename)
    {
        _mpegFile = new MpegFile(filename);
    }

    public MpegFileAdapter(Stream stream)
    {
        _mpegFile = new MpegFile(stream);
    }

    public override void GetInfo(out int sampleRate, out int bitsPerSample, out int channels, out float duration)
    {
        sampleRate = _mpegFile.SampleRate;
        bitsPerSample = 16;
        channels = _mpegFile.Channels;
        duration = (float)_mpegFile.Duration.TotalSeconds;
    }

    public override int Read(byte[] buffer, int length)
    {
        return _mpegFile.ReadSamplesInt16(buffer, 0, length);
    }

    public override void Seek(int position)
    {
        _mpegFile.Position = position;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _mpegFile.Dispose();
            }

            _disposed = true;
        }
    }
}