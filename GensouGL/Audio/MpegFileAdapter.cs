namespace GensouGL.Audio;

internal sealed class MpegFileAdapter : StreamingSource
{
    private MpegFile _mpegFile;
    private Stream _fileStream;

    public MpegFileAdapter(string filename)
    {
        _fileStream = GensouFileSystem.OpenReadFile(filename);
        _mpegFile = new MpegFile(_fileStream);
    }

    public MpegFileAdapter(Stream stream)
    {
        _mpegFile = new MpegFile(stream);
    }

    public override AudioInfo GetInfo()
    {
        return new AudioInfo((float)_mpegFile.Duration.TotalSeconds, _mpegFile.SampleRate, 16, _mpegFile.Channels);
    }

    public override int Read(byte[] buffer, int length)
    {
        return _mpegFile.ReadSamplesInt16(buffer, 0, length);
    }

    public override void Seek(long position)
    {
        _mpegFile.Position = position;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _mpegFile?.Dispose();
                _fileStream?.Dispose();
            }

            _disposed = true;
        }
    }
}