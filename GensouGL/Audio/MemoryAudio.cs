using System.Threading.Channels;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Audio;

/// <summary>
/// Represents an audio clip loaded entirely into memory, providing playback control and seeking capabilities.
/// </summary>
/// <remarks><see cref="MemoryAudio"/> enables low-latency playback and random access to audio data by storing the entire audio
/// stream in memory. Use the static From* methods to create instances from supported audio sources such as MP3 or WAV
/// files or streams. This class implements <see cref="IDisposable"/>; callers should dispose of instances when no longer needed to
/// release unmanaged resources. <see cref="MemoryAudio"/> is not thread-safe; synchronize access if used from multiple
/// threads.</remarks>
public sealed class MemoryAudio : Audio, IDisposable
{
    private int _buffer;
    
    private bool _disposed = false;

    /// <summary>
    /// Gets the OpenAL buffer handle.
    /// </summary>
    public int Buffer => _buffer;
    /// <summary>
    /// Gets the current playback position in seconds.
    /// </summary>
    public float Position => AL.GetSource(_source, ALSourcef.SecOffset);

    /// <summary>
    /// Gets or sets whether the audio loops.
    /// </summary>
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


    /// <summary>
    /// Starts playback of the audio source.
    /// </summary>
    /// <remarks>If the audio source is already playing, this method stops the audio and play it from the beginning. Use this method to resume
    /// playback after pausing or to start playback from the beginning if the source is stopped.</remarks>
    public void Play()
    {
        AL.SourcePlay(_source);
    }

    /// <summary>
    /// Pauses playback of the current audio source if it is playing.
    /// </summary>
    /// <remarks>Calling this method has no effect if the audio source is already paused or stopped. To resume
    /// playback after pausing, use the play method.</remarks>
    public void Pause()
    {
        AL.SourcePause(_source);
    }

    /// <summary>
    /// Stops playback of the audio source if it is currently playing.
    /// </summary>
    /// <remarks>Calling this method halts any ongoing audio playback associated with the source. If the
    /// source is already stopped, this method has no effect. To resume playback, use the appropriate play
    /// method.</remarks>
    public void Stop()
    {
        AL.SourceStop(_source);
    }

    /// <summary>
    /// Seeks the playback position to the specified position in seconds.
    /// </summary>
    /// <param name="position">Position to which the playback position should be sought</param>
    public void Seek(float position)
    {
        if (position < 0 || position >= Duration)
        {
            DebugLog.Error($"{nameof(MemoryAudio)}: Specified position is out of range.");
            return;
        }

        AL.Source(_source, ALSourcef.SecOffset, position);
    }








    /// <summary>
    /// Initializes a new instance of <see cref="MemoryAudio"/> using the specified <see cref="AudioSource"/>.
    /// </summary>
    /// <param name="source">AudioSource instance to be used to initialize <see cref="MemoryAudio"/>.</param>
    /// <returns>Instance of <see cref="MemoryAudio"/>.</returns>
    public static MemoryAudio From(AudioSource source)
    {
        (byte[] pcm, AudioInfo info) = source.CreateCompleteData();
        return BufferCreate(pcm, info);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="MemoryAudio"/> using the specified MP3 file name.
    /// </summary>
    /// <param name="filename">File name to be used to initialize <see cref="MemoryAudio"/>.</param>
    /// <returns>Instance of <see cref="MemoryAudio"/>.</returns>
    public static MemoryAudio FromMp3(string filename)
         => From(AudioSource.FromMp3(filename));

    /// <summary>
    /// Initializes a new instance of <see cref="MemoryAudio"/> using the specified MP3 stream.
    /// </summary>
    /// <param name="stream">MP3 stream to be used to initialize <see cref="MemoryAudio"/>.</param>
    /// <returns>Instance of <see cref="MemoryAudio"/>.</returns>
    public static MemoryAudio FromMp3(Stream stream)
        => From(AudioSource.FromMp3(stream));

    /// <summary>
    /// Initializes a new instance of <see cref="MemoryAudio"/> using the specified Wave file name.
    /// </summary>
    /// <param name="filename">File name to be used to initialize <see cref="MemoryAudio"/>.</param>
    /// <returns>Instance of <see cref="MemoryAudio"/>.</returns>
    public static MemoryAudio FromWave(string filename)
        => From(AudioSource.FromWave(filename));

    /// <summary>
    /// Initializes a new instance of <see cref="MemoryAudio"/> using the specified Wave stream.
    /// </summary>
    /// <param name="stream">Wave stream to be used to initialize <see cref="MemoryAudio"/>.</param>
    /// <returns>Instance of <see cref="MemoryAudio"/>.</returns>
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

    /// <summary>
    /// Releases all resources contained by <see cref="MemoryAudio"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="MemoryAudio"/> destructor.
    /// </summary>
    ~MemoryAudio()
    {
        Dispose(false);
    }
}