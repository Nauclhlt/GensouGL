using System.Threading.Channels;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Audio;

/// <summary>
/// Provides information about an audio stream, including duration, sample rate, bit depth, and channel count.
/// </summary>
/// <remarks>Use this structure to describe the basic properties of an audio file or stream. The values are
/// typically used for audio processing, playback configuration, or format conversion.</remarks>
public readonly struct AudioInfo
{
    /// <summary>
    /// Duration of audio.
    /// </summary>
    public readonly float Duration;
    /// <summary>
    /// Sample rate of audio.
    /// </summary>
    public readonly int SampleRate;
    /// <summary>
    /// Value representing the number of bits used to represent each audio sample.
    /// </summary>
    public readonly int BitsPerSample;
    /// <summary>
    /// Value representing the number of channels represented by this instance.
    /// </summary>
    public readonly int Channels;

    /// <summary>
    /// Initializes <see cref="AudioInfo" /> with given parameters.
    /// </summary>
    /// <param name="duration">Duration of audio.</param>
    /// <param name="sampleRate">Sample rate of audio.</param>
    /// <param name="bitsPerSample">Value representing the number of bits used to represent each audio sample.</param>
    /// <param name="channels">Value representing the number of channels represented by this instance.</param>
    public AudioInfo(float duration, int sampleRate, int bitsPerSample, int channels)
    {
        Duration = duration;
        SampleRate = sampleRate;
        BitsPerSample = bitsPerSample;
        Channels = channels;
    }

    internal ALFormat ToFormat()
    {
        return (Channels, BitsPerSample) switch
        {
            (1, 8) => ALFormat.Mono8,
            (2, 8) => ALFormat.Stereo8,
            (1, 16) => ALFormat.Mono16,
            (2, 16) => ALFormat.Stereo16,
            _ => ALFormat.Mono16
        };
    }

    /// <summary>
    /// Converts a byte count to the equivalent duration in seconds based on the current audio format settings.
    /// </summary>
    /// <remarks>The calculation uses the current values of SampleRate, Channels, and BitsPerSample to
    /// determine the byte rate. Ensure these properties are set appropriately for accurate conversion.</remarks>
    /// <param name="bytes">The number of bytes to convert to seconds. Must be non-negative.</param>
    /// <returns>The duration, in seconds, represented by the specified number of bytes.</returns>
    public readonly float BytesToSeconds(long bytes)
    {
        float bytesPerSecond = SampleRate * Channels * (BitsPerSample / 8.0f);
        return bytes / bytesPerSecond;
    }

    /// <summary>
    /// Calculates the number of bytes required to represent the specified duration of audio data, based on the current
    /// sample rate, channel count, and bits per sample.
    /// </summary>
    /// <param name="seconds">The duration of audio, in seconds, to convert to a byte count. Must be greater than or equal to 0.</param>
    /// <returns>The number of bytes corresponding to the specified duration of audio data.</returns>
    public readonly long SecondsToBytes(float seconds)
    {
        float bytesPerSecond = SampleRate * Channels * (BitsPerSample / 8.0f);
        return (long)(seconds * bytesPerSecond);
    }
}