using System.Threading.Channels;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

public sealed class MemoryAudio : Audio, IDisposable
{
    private int _buffer;
    
    private bool _disposed = false;

    public int Buffer => _buffer;
    public float Position => AL.GetSource(_source, ALSourcef.SecOffset);

    public bool Loop
    {
        get => AL.GetSource(_source, ALSourceb.Looping);
        set => AL.Source(_source, ALSourceb.Looping, value);
    }

    private MemoryAudio()
    {
    }

    private MemoryAudio(int buffer, int source, AudioInfo info)
    {
        _buffer = buffer;
        _source = source;
        _info = info;
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
        if (position < 0 || position >= Duration)
        {
#if DEBUG
            DebugLog.Error("MemoryAudio: Specified position is out of range.");
#endif
            return;
        }

        AL.Source(_source, ALSourcef.SecOffset, position);
    }








    public static MemoryAudio From(AudioSource source)
    {
        (byte[] pcm, AudioInfo info) = source.CreateCompleteData();
        return BufferCreate(pcm, info);
    }

    public static MemoryAudio FromMp3(string filename)
         => From(AudioSource.FromMp3(filename));

    public static MemoryAudio FromMp3(Stream stream)
        => From(AudioSource.FromMp3(stream));

    public static MemoryAudio FromWave(string filename)
        => From(AudioSource.FromWave(filename));

    public static MemoryAudio FromWave(Stream stream)
        => From(AudioSource.FromWave(stream));

    private static MemoryAudio BufferCreate(byte[] pcm, AudioInfo info)
    {
        int buffer = AL.GenBuffer();
        int source = AL.GenSource();

        ALFormat format = info.Channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16;
        AL.BufferData(buffer, format, pcm, info.SampleRate);

        AL.Source(source, ALSourcei.Buffer, buffer);

        return new(buffer, source, info);
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