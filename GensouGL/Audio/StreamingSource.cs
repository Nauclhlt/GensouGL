namespace GensouGL.Audio;

/// <summary>
/// Represents an abstract base class for sources providing methods used for streaming playback of audio.
/// </summary>
/// <remarks>
/// The StreamingSource class implements <see cref="IDisposable "/> interface, so derived classes must override <see cref="Dispose(bool)"/> to release resources.
/// </remarks>
public abstract class StreamingSource : IDisposable
{
    /// <summary>
    /// Whether the instance is disposed.
    /// </summary>
    protected bool _disposed = false;

    /// <summary>
    /// When overridden, releases all resources contained by <see cref="StreamingSource"/>.
    /// </summary>
    /// <param name="disposing"></param>
    protected abstract void Dispose(bool disposing);

    /// <summary>
    /// Reads the audio data of the length to buffer, if the length of the remaining data is less than length, reads the rest of data and returns the length of it.
    /// </summary>
    /// <param name="buffer">Buffer array to which the data should be loaded.</param>
    /// <param name="length">Length to be loaded.</param>
    /// <returns>Value representing the actual length of data loaded.</returns>
    public abstract int Read(byte[] buffer, int length);
    /// <summary>
    /// When overridden, seeks the current position to the specified position.
    /// </summary>
    /// <param name="position">Position to which the current position should be sought.</param>
    public abstract void Seek(long position);
    /// <summary>
    /// When overridden, returns the <see cref="AudioInfo"/> instance representing the audio information.
    /// </summary>
    /// <returns><see cref="AudioInfo"/> instance.</returns>
    public abstract AudioInfo GetInfo();

    /// <summary>
    /// Releases all resources contained by <see cref="StreamingSource"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="StreamingSource"/> destructor.
    /// </summary>
    ~StreamingSource()
    {
        Dispose(false);
    }
}