using System.Threading.Channels;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

public readonly struct AudioInfo
{
    public readonly float Duration;
    public readonly int SampleRate;
    public readonly int BitsPerSample;
    public readonly int Channels;

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

    public readonly float BytesToSeconds(long bytes)
    {
        float bytesPerSecond = SampleRate * Channels * (BitsPerSample / 8.0f);
        return bytes / bytesPerSecond;
    }

    public readonly long SecondsToBytes(float seconds)
    {
        float bytesPerSecond = SampleRate * Channels * (BitsPerSample / 8.0f);
        return (long)(seconds * bytesPerSecond);
    }
}