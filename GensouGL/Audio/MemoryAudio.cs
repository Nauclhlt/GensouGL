using System.Threading.Channels;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

public sealed class MemoryAudio : Audio, IDisposable
{
    private int _buffer;
    
    private bool _disposed = false;

    public int Buffer => _buffer;

    public bool Loop
    {
        get => AL.GetSource(_source, ALSourceb.Looping);
        set => AL.Source(_source, ALSourceb.Looping, value);
    }

    private MemoryAudio()
    {
    }

    private MemoryAudio(int buffer, int source, int sampleRate, int channels, int bitsPerSample, float duration)
    {
        _buffer = buffer;
        _source = source;
        _sampleRate = sampleRate;
        _channels = channels;
        _bitsPerSample = bitsPerSample;
        _duration = duration;
    }


    public void Play()
    {
        AL.SourcePlay(_source);
    }

    public void Pause()
    {
        AL.SourcePause(_source);
    }

    public void Stop()
    {
        AL.SourceStop(_source);
    }

    public void Seek(float position)
    {
        if (position < 0 || position >= _duration)
        {
#if DEBUG
            DebugLog.Error("MemoryAudio: Specified position is out of range.");
#endif
            return;
        }

        AL.Source(_source, ALSourcef.SecOffset, position);
    }









    public static MemoryAudio FromMp3(string filename)
    {
        using (var fs = File.OpenRead(filename))
        {
            return FromMp3(fs);
        }
    }

    public static MemoryAudio FromMp3(Stream stream)
    {
        var mp3 = new MpegFile(stream);
        int sampleRate = mp3.SampleRate;
        int channels = mp3.Channels;

        byte[] pcm = new byte[mp3.Length];
        int read = mp3.ReadSamplesInt16(pcm, 0, pcm.Length);

        return BufferCreate(pcm, channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16, sampleRate, channels, 16, (float)mp3.Duration.TotalSeconds);
    }

    public static MemoryAudio FromWave(string filename)
    {
        using (var fs = File.OpenRead(filename))
        {
            return FromWave(fs);
        }
    }

    public static MemoryAudio FromWave(Stream stream)
    {
        WavPcmDecoder decoder = new();
        decoder.Load(stream);

        return BufferCreate(decoder.PcmData, Audio.ToFormat(decoder.Channels, decoder.BitsPerSample), decoder.SampleRate, decoder.Channels, decoder.BitsPerSample, decoder.Duration);
    }

    private static MemoryAudio BufferCreate(byte[] pcm, ALFormat format, int sampleRate, int channels, int bitsPerSample, float duration)
    {
        int buffer = AL.GenBuffer();
        int source = AL.GenSource();

        AL.BufferData(buffer, format, pcm, sampleRate);

        AL.Source(source, ALSourcei.Buffer, buffer);

        return new(buffer, source, sampleRate, channels, bitsPerSample, duration);
    }

    private void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                
            }

            AL.DeleteBuffer(_buffer);
            AL.DeleteSource(_source);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MemoryAudio()
    {
        Dispose(false);
    }
}