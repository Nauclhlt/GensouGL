namespace GensouGL.Audio;

using NVorbis;

internal sealed class OggVorbisAdapter : StreamingSource
{
    private VorbisReader _reader;
    private Stream _stream;

    public OggVorbisAdapter(string filename)
    {
        _stream = GensouFileSystem.OpenReadFile(filename);
        _reader = new VorbisReader(_stream, false);
    }

    public OggVorbisAdapter(Stream stream)
    {
        _stream = stream;
        _reader = new VorbisReader(stream, false);
    }

    public override AudioInfo GetInfo()
    {
        return new AudioInfo(
            (float)_reader.TotalTime.TotalSeconds,
            _reader.SampleRate,
            16,
            _reader.Channels
        );
    }

    public override int Read(byte[] buffer, int length)
    {
        int samplesToRead = length / 2;

        float[] temp = new float[samplesToRead];
        int samplesRead = _reader.ReadSamples(temp, 0, samplesToRead);

        for (int i = 0; i < samplesRead; i++)
        {
            float f = temp[i];

            // clamp
            if (f > 1f) f = 1f;
            if (f < -1f) f = -1f;

            short s = (short)(f * 32767);

            buffer[i * 2] = (byte)(s & 0xff);
            buffer[i * 2 + 1] = (byte)((s >> 8) & 0xff);
        }

        return samplesRead * 2;
    }

    public override void Seek(long position)
    {
        long samplePos = position / 2;
        _reader.SamplePosition = samplePos;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _reader?.Dispose();
                _stream?.Dispose();
            }

            _disposed = true;
        }
    }
}