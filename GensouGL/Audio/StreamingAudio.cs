using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace GensouGL.Core;

// For longer audio.
public sealed class StreamingAudio : Audio, IDisposable
{
    private const int BufferCount = 3;
    private const int BufferSize = 4096 * 4;

    private int[] _buffers;
    private StreamingSource _streamingSource;
    private ALFormat _format;
    private bool _loop;
    private AudioState _state;
    private int _buffersPlayed;
    // Used to prevent duplicate playback...
    private Guid _uniqueId;


    private bool _disposed = false;


    public Guid UniqueId => _uniqueId;
    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }
    public float Position => BytesToSeconds((long)_buffersPlayed * BufferSize) * AL.GetSource(_source, ALSourcef.SecOffset);
    public AudioState State => _state;
    public int[] Buffers => _buffers;

    private StreamingAudio()
    {
    }

    private StreamingAudio(StreamingSource streamingSource)
    {
        _uniqueId = Guid.NewGuid();
        _streamingSource = streamingSource;
        _streamingSource.GetInfo(out _sampleRate, out _bitsPerSample, out _channels, out _duration);
        _format = Audio.ToFormat(_channels, _bitsPerSample);
        _loop = false;
        _state = AudioState.Stopped;
        _buffersPlayed = 0;

        _source = AL.GenSource();

        _buffers = new int[BufferCount];
        for (int i = 0; i < BufferCount; i++)
        {
            _buffers[i] = AL.GenBuffer();
        }
    }


    private bool QueueNewBuffer(int buffer)
    {
        byte[] buf = new byte[BufferSize];
        int read = _streamingSource.Read(buf, BufferSize);
        if (read <= 0) return false;

        AL.BufferData(buffer, _format, ref buf[0], read, _sampleRate);
        AL.SourceQueueBuffer(_source, buffer);

        return true;
    }



    public void Play()
    {
        if (_state == AudioState.Stopped)
        {
            _buffersPlayed = 0;
            _state = AudioState.Playing;

            _streamingSource.Seek(0);
            
            for (int i = 0; i < BufferCount; i++)
            {
                QueueNewBuffer(_buffers[i]);
            }

            AudioPlayer.StreamingPlay(this);
        }
        else if (_state == AudioState.Paused)
        {
            AL.SourcePlay(_source);
        }
        else
        {
#if DEBUG
            DebugLog.Warn("StreamingAudio: Play() called during playback.");
#endif
        }
    }

    public void Pause()
    {
        _state = AudioState.Paused;
        AL.SourcePause(_source);
    }

    public void Stop()
    {
        if (_state == AudioState.Stopped)
        {
#if DEBUG
            DebugLog.Warn("StreamingAudio: Stop() called before playback.");
#endif
            return;
        }

        _state = AudioState.Stopped;
        AudioPlayer.StreamingStop(this);
        _buffersPlayed = 0;

        int queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
        while (queued-- > 0)
        {
            int buf = AL.SourceUnqueueBuffer(_source);
        }

        AL.Source(_source, ALSourcef.SecOffset, 0f);
    }

    public void Seek(float position)
    {
        if (position < 0 || position >= _duration)
        {
#if DEBUG
            DebugLog.Error("StreamingAudio: Specified position is out of range.");
#endif
            return;
        }

        // TODO: implement.
    }

    internal void Update()
    {
        int proc = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);

        while (proc-- > 0)
        {
            int buffer = AL.SourceUnqueueBuffer(_source);
            _buffersPlayed++;

            if (!QueueNewBuffer(buffer))
            {
                // end
                if (_loop)
                {
                    _streamingSource.Seek(0);
                    QueueNewBuffer(buffer);
                }
                else
                {
                    Stop();
                    return;
                }
            }
        }

        if ((ALSourceState)AL.GetSource(_source, ALGetSourcei.SourceState) != ALSourceState.Playing)
        {
            if (_state == AudioState.Playing)
                AL.SourcePlay(_source);
        }
    }

    private float BytesToSeconds(long bytes)
    {
        float bytesPerSecond = _sampleRate * _channels * (_bitsPerSample / 8.0f);
        return bytes / bytesPerSecond;
    }

    private long SecondsToBytes(float seconds)
    {
        float bytesPerSecond = _sampleRate * _channels * (_bitsPerSample / 8.0f);
        return (long)(seconds * bytesPerSecond);
    }




    public static StreamingAudio FromMp3(string filename)
    {
        return new StreamingAudio(new MpegFileAdapter(filename));
    }

    public static StreamingAudio FromMp3(Stream stream)
    {
        return new StreamingAudio(new MpegFileAdapter(stream));
    }

    public static StreamingAudio FromWave(string filename)
    {
        return new StreamingAudio(new WavePcmSource(filename));
    }

    public static StreamingAudio FromWave(Stream stream)
    {
        return new StreamingAudio(new WavePcmSource(stream));
    }


    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _streamingSource.Dispose();
            }

            AL.DeleteSource(_source);
            for (int i = 0; i < BufferCount; i++)
            {
                AL.DeleteBuffer(_buffers[i]);
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~StreamingAudio()
    {
        Dispose(false);
    }
}