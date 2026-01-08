using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace GensouGL.Audio;

/// <summary>
/// Represents an audio clip that supports streaming playback, providing playback control and seeking capabilities.
/// </summary>
/// <remarks><see cref="StreamingAudio"/> enables streaming playback, which provides the memory-efficient way to play longer audio.
/// Use the static From* methods to create instances from supported audio sources such as MP3 or WAV
/// files or streams. This class implements <see cref="IDisposable"/>; callers should dispose of instances when no longer needed to
/// release unmanaged resources. <see cref="StreamingAudio"/> is not thread-safe; synchronize access if used from multiple
/// threads.</remarks>
public sealed class StreamingAudio : Audio, IDisposable
{
    private const int BufferCount = 3;
    private const int BufferSize = 4096 * 4;

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


    /// <summary>
    /// Gets or sets whether the audio loops.
    /// </summary>
    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    /// <summary>
    /// Gets or sets the start position of one loop in seconds. Value must be within the valid range.
    /// </summary>
    public float LoopStart
    {
        get => _loopStart;
        set
        {
            if (_state != AudioState.Stopped)
            {
                DebugLog.Warn($"{nameof(StreamingAudio)}: Do not set LoopStart or LoopEnd while playing or being paused. Setter skipped.");
                return;
            }

            if (value < 0 || value > Duration)
            {
                return;
            }

            _loopStart = value;
        }
    }

    /// <summary>
    /// Gets or sets the end position of one loop in seconds. Value must be within the valid range.
    /// </summary>
    public float LoopEnd
    {
        get => _loopEnd;
        set
        {
            if (_state != AudioState.Stopped)
            {
                DebugLog.Warn($"{nameof(StreamingAudio)}: Do not set LoopStart or LoopEnd while playing or being paused. Setter skipped.");
                return;
            }

            if (_loopEnd < 0)
            {
                return;
            }

            _loopEnd = value > Duration ? float.PositiveInfinity : value;
        }
    }
    
    
    /// <summary>
    /// Gets the current playback position in seconds.
    /// </summary>
    public float Position => _info.BytesToSeconds((long)_buffersPlayed * BufferSize) + AL.GetSource(_source, ALSourcef.SecOffset);    
    /// <summary>
    /// Gets the current state of the audio.
    /// </summary>
    public AudioState State => _state;
    /// <summary>
    /// Gets the unique ID associated with the audio.
    /// </summary>
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


    /// <summary>
    /// Plays the audio.
    /// </summary>
    /// <remarks>
    /// When the current state is Playing: has no effect. When the current state is Paused: resumes the playback from the last position. 
    /// When the current state is Stopped: plays the audio from the beginning.
    /// </remarks>
    public void Play()
    {
        if (LoopStart > LoopEnd)
        {
            DebugLog.Error($"{nameof(StreamingAudio)}: LoopStart > LoopEnd.");
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
            DebugLog.Warn($"{nameof(StreamingAudio)}: Play() called during playback.");
        }
    }

    /// <summary>
    /// Pauses the audio.
    /// </summary>
    /// <remarks>
    /// Paused audio can be resumed by the play method.
    /// </remarks>
    public void Pause()
    {
        if (_state == AudioState.Stopped)
        {
            DebugLog.Warn($"{nameof(StreamingAudio)}: Pause() called, but the audio was not playing.");
        }
        _state = AudioState.Paused;
        AL.SourcePause(_source);
    }

    /// <summary>
    /// Stops the audio.
    /// </summary>
    public void Stop()
    {
        if (_state == AudioState.Stopped)
        {
            DebugLog.Warn($"{nameof(StreamingAudio)}: Stop() called before playback.");
            return;
        }

        _state = AudioState.Stopped;
        AudioPlayer.StreamingStop(this);
        _buffersPlayed = 0;
        _streamingSource.Seek(0);

        DequeueAllBuffers();

        AL.Source(_source, ALSourcef.SecOffset, 0f);
    }

    /// <summary>
    /// Seeks the playback position to the specified position in seconds.
    /// </summary>
    /// <param name="position">Position to which the playback position should be sought</param>
    public void Seek(float position)
    {
        if (position < 0 || position >= Duration)
        {
            DebugLog.Error($"{nameof(StreamingAudio)}: Specified position is out of range.");
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




    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio"/> using the specified MP3 file name.
    /// </summary>
    /// <param name="filename">File name to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio FromMp3(string filename)
    {
        return new StreamingAudio(new MpegFileAdapter(filename));
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio "/> using the specified MP3 stream.
    /// </summary>
    /// <param name="stream">MP3 stream to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio FromMp3(Stream stream)
    {
        return new StreamingAudio(new MpegFileAdapter(stream));
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio"/> using the specified Wave file name.
    /// </summary>
    /// <param name="filename">File name to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio FromWave(string filename)
    {
        return new StreamingAudio(new WavePcmSource(filename));
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio"/> using the specified Wave stream.
    /// </summary>
    /// <param name="stream">Wave stream to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio FromWave(Stream stream)
    {
        return new StreamingAudio(new WavePcmSource(stream));
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio"/> using the specified <see cref="AudioSource"/>.
    /// </summary>
    /// <param name="source"><see cref="AudioSource"/> instance to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio From(AudioSource source)
    {
        return new StreamingAudio(source.CreateStreamingSource());
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StreamingAudio"/> using the specified <see cref="StreamingSource"/>.
    /// </summary>
    /// <param name="streamingSource"><see cref="StreamingSource"/> instance to be used to initialize <see cref="StreamingAudio"/>.</param>
    /// <returns>Instance of <see cref="StreamingAudio"/>.</returns>
    public static StreamingAudio FromStreamingSource(StreamingSource streamingSource)
    {
        return new StreamingAudio(streamingSource);
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

    /// <summary>
    /// Releases all resources contained by <see cref="StreamingAudio"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="StreamingAudio"/> destructor.
    /// </summary>
    ~StreamingAudio()
    {
        Dispose(false);
    }
}