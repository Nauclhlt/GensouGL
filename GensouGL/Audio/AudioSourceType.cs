namespace GensouGL.Audio;

/// <summary>
/// Specifies the type of audio source used for playback or processing.
/// </summary>
/// <remarks>Use this enumeration to indicate whether the audio data originates from a file or a stream, and
/// whether it is in MP3 or WAV format. The value determines how the audio is loaded and handled by the consuming
/// API.</remarks>
public enum AudioSourceType
{
    /// <summary>
    /// From a MP3 file.
    /// </summary>
    Mp3File,
    /// <summary>
    /// From a Wave file.
    /// </summary>
    WaveFile,
    /// <summary>
    /// From a Ogg file.
    /// </summary>
    OggFile,
    /// <summary>
    /// From a MP3 stream.
    /// </summary>
    Mp3Stream,
    /// <summary>
    /// From a Wave stream.
    /// </summary>
    WaveStream,
    /// <summary>
    /// From a Ogg stream.
    /// </summary>
    OggStream
}