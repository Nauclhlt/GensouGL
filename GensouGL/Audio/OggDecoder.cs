namespace GensouGL.Audio;

using NVorbis;

internal static class OggDecoder
{
    public static (byte[] pcm, AudioInfo info) Load(string filename)
    {
        using var stream = GensouFileSystem.OpenReadFile(filename);
        return Load(stream, true);
    }

    public static (byte[] pcm, AudioInfo info) Load(Stream stream, bool close = false)
    {
        using var reader = new VorbisReader(stream, close);

        int channels = reader.Channels;
        int sampleRate = reader.SampleRate;

        List<byte> result = new();

        float[] buffer = new float[4096];

        while (true)
        {
            int samplesRead = reader.ReadSamples(buffer, 0, buffer.Length);
            if (samplesRead == 0) break;

            for (int i = 0; i < samplesRead; i++)
            {
                float f = buffer[i];

                // clamp
                if (f > 1f) f = 1f;
                if (f < -1f) f = -1f;

                short s = (short)(f * 32767);

                result.Add((byte)(s & 0xff));
                result.Add((byte)((s >> 8) & 0xff));
            }
        }

        return (result.ToArray(), new AudioInfo((float)reader.TotalTime.TotalSeconds, sampleRate, 16, channels));
    }
}