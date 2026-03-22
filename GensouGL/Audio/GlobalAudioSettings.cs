namespace GensouGL.Audio;

/// <summary>
/// Contains global settings for audio playback.
/// </summary>
public static class GlobalAudioSettings
{
    /// <summary>
    /// Gets or sets the size of streaming buffers.
    /// </summary>
    public static int StreamingBufferSize
    {
        get; set;
    } = 4096 * 10;

    /// <summary>
    /// Gets or sets the number of buffers queued when streaming audio.
    /// </summary>
    public static int StreamingBufferCount
    {
        get; set;
    } = 5;
}