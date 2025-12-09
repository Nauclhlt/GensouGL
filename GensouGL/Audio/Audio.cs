using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

public abstract class Audio
{
    protected int _source;
    protected float _duration;
    protected int _sampleRate;
    protected int _bitsPerSample;
    protected int _channels;

    public int Source => _source;
    public float Duration => _duration;
    public int SampleRate => _sampleRate;
    public int Channels => _channels;
    public int BitsPerSample => _bitsPerSample;

    public float Volume
    {
        get => AL.GetSource(_source, ALSourcef.Gain);
        set
        {
            if (value < 0)
            {
#if DEBUG
                DebugLog.Error("Audio: Volume cannot be negative.");
#endif
                return;
            }

            AL.Source(_source, ALSourcef.Gain, value);
        }
    }

    public float Pitch
    {
        get => AL.GetSource(_source, ALSourcef.Pitch);
        set
        {
            if (value < 0.5f || value > 2.0f)
            {
#if DEBUG
                DebugLog.Error("Audio: Pitch value must be in the range [0.5, 2.0]");
#endif
                return;
            }

            AL.Source(_source, ALSourcef.Pitch, value);
        }
    }


    internal static ALFormat ToFormat(int channels, int bitsPerSample)
    {
        return (channels, bitsPerSample) switch
        {
            (1, 8) => ALFormat.Mono8,
            (2, 8) => ALFormat.Stereo8,
            (1, 16) => ALFormat.Mono16,
            (2, 16) => ALFormat.Stereo16,
            _ => ALFormat.Mono16
        };
    }
}