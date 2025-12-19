using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

public abstract class Audio
{
    protected int _source;
    protected AudioInfo _info;

    public int Source => _source;
    public float Duration => _info.Duration;
    public int SampleRate => _info.SampleRate;
    public int Channels => _info.Channels;
    public int BitsPerSample => _info.BitsPerSample;

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
}