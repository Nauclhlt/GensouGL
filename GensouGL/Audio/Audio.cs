using OpenTK.Audio.OpenAL;

namespace GensouGL.Audio;

/// <summary>
/// Represents an abstract base class for audio resources, providing common properties for playback control and audio
/// metadata.
/// </summary>
/// <remarks>The Audio class exposes properties for controlling playback parameters such as volume and pitch, as
/// well as accessing audio format information. Derived classes implement specific audio playback or manipulation
/// functionality. This class does not provide playback methods directly; use derived types to play or manage audio
/// content.</remarks>
public abstract class Audio
{
    /// <summary>
    /// OpenAL audio source handle.
    /// </summary>
    protected int _source;
    /// <summary>
    /// Audio metadata.
    /// </summary>
    protected AudioInfo _info;

    /// <summary>
    /// Gets the OpenAL audio source handle.
    /// </summary>
    public int Source => _source;
    /// <summary>
    /// Gets the duration in seconds.
    /// </summary>
    public float Duration => _info.Duration;
    /// <summary>
    /// Gets the sample rate.
    /// </summary>
    public int SampleRate => _info.SampleRate;
    /// <summary>
    /// Gets the number of channels. 1: monoral audio, 2: stereo audio.
    /// </summary>
    public int Channels => _info.Channels;
    /// <summary>
    /// Gets the bits-per-sample value.
    /// </summary>
    public int BitsPerSample => _info.BitsPerSample;

    /// <summary>
    /// Gets or sets the volume of the audio. Volume must be non-negative.
    /// </summary>
    public float Volume
    {
        get => AL.GetSource(_source, ALSourcef.Gain);
        set
        {
            if (value < 0)
            {
                DebugLog.Error($"{nameof(Audio)}: Volume cannot be negative.");
                return;
            }

            AL.Source(_source, ALSourcef.Gain, value);
        }
    }

    /// <summary>
    /// Gets or sets the pitch of the audio. Pitch must be within the range [0.5, 2.0]
    /// </summary>
    public float Pitch
    {
        get => AL.GetSource(_source, ALSourcef.Pitch);
        set
        {
            if (value < 0.5f || value > 2.0f)
            {
                DebugLog.Error($"{nameof(Audio)}: Pitch value must be in the range [0.5, 2.0]");
                return;
            }

            AL.Source(_source, ALSourcef.Pitch, value);
        }
    }
}