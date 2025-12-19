using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace GensouGL.Core;

// For longer audio.
public sealed class StreamingAudio : Audio, IDisposable
{
    private const int BufferCount = 3;
    private const int BufferSize = 4096 * 4;

    // Used to prevent duplicate playback...
    private Guid _uniqueId;
    private int[] _buffers;
    private StreamingSource _streamingSource;
    private ALFormat _format;
    private AudioState _state;
    private long _buffersPlayed;
    

    private bool _loop;
    private float _loopStart = 0f;
    private float _loopEnd = float.PositiveInfinity;


    private bool _disposed = false;


    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    public float LoopStart
    {
        get => _loopStart;
        set
        {
            if (_state != AudioState.Stopped)
            {
#if DEBUG
                DebugLog.Warn("StreamingAudio: Do not set LoopStart or LoopEnd while playing or being paused. Setter skipped.");
#endif
                return;
            }

            if (value < 0 || value > Duration)
            {
                return;
            }

            _loopStart = value;
        }
    }

    public float LoopEnd
    {
        get => _loopEnd;
        set
        {
            if (_state != AudioState.Stopped)
            {
#if DEBUG
                DebugLog.Warn("StreamingAudio: Do not set LoopStart or LoopEnd while playing or being paused. Setter skipped.");
#endif
                return;
            }

            if (_loopEnd < 0)
            {
                return;
            }

            _loopEnd = value > Duration ? float.PositiveInfinity : value;
        }
    }
    
    
    public float Position => _info.BytesToSeconds((long)_buffersPlayed * BufferSize) + AL.GetSource(_source, ALSourcef.SecOffset);    
    public AudioState State => _state;
    public int[] Buffers => _buffers;
    public Guid UniqueId => _uniqueId;


    private StreamingAudio()
    {
    }

    private StreamingAudio(StreamingSource streamingSource)
    {
        _uniqueId = Guid.NewGuid();
        _streamingSource = streamingSource;
        _info = _streamingSource.GetInfo();
        _format = _info.ToFormat();
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

        AL.BufferData(buffer, _format, ref buf[0], read, SampleRate);
        AL.SourceQueueBuffer(_source, buffer);

        return true;
    }

    private void DequeueAllBuffers()
    {
        int queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
        while (queued-- > 0)
        {
            AL.SourceUnqueueBuffer(_source);
        }
    }



    public void Play()
    {
        if (LoopStart > LoopEnd)
        {
#if DEBUG
            DebugLog.Error("StreamingAudio: LoopStart > LoopEnd.");
#endif
            return;
        }

        if (_state == AudioState.Stopped)
        {
            _buffersPlayed = 0;
            _state = AudioState.Playing;

            DequeueAllBuffers();
            
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
        _streamingSource.Seek(0);

        DequeueAllBuffers();

        AL.Source(_source, ALSourcef.SecOffset, 0f);
    }

    public void Seek(float position)
    {
        if (position < 0 || position >= Duration)
        {
#if DEBUG
            DebugLog.Error("StreamingAudio: Specified position is out of range.");
#endif
            return;
        }

        if (_state != AudioState.Stopped)
        {
            AL.SourceStop(_source);
        }

        DequeueAllBuffers();

        long offset = _info.SecondsToBytes(position);
        _streamingSource.Seek(offset);
        _buffersPlayed = offset / BufferSize;

        if (_state != AudioState.Stopped)
        {
            for (int i = 0; i < BufferCount; i++)
            {
                QueueNewBuffer(_buffers[i]);
            }

            if (_state == AudioState.Playing) AL.SourcePlay(_source);
        }
    }

    private void NextLoop()
    {
        AL.SourceStop(_source);

        DequeueAllBuffers();

        long start = _info.SecondsToBytes(LoopStart);
        _streamingSource.Seek(start);
        _buffersPlayed = start / BufferSize;

        for (int i = 0; i < BufferCount; i++)
        {
            QueueNewBuffer(_buffers[i]);
        }

        AL.SourcePlay(_source);
    }

    internal void Update()
    {
        int proc = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);

        if (Position >= LoopEnd)
        {
            NextLoop();
            return;
        }

        while (proc-- > 0)
        {
            int buffer = AL.SourceUnqueueBuffer(_source);
            _buffersPlayed++;

            if (!QueueNewBuffer(buffer))
            {
                // end
                if (_loop)
                {
                    NextLoop();
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

    public static StreamingAudio From(AudioSource source)
    {
        return new StreamingAudio(source.CreateStreamingSource());
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