using System.IO;

namespace GensouGL.Core;

public sealed class AudioSource
{
    private AudioSourceType _type;
    private string _filename;
    private Stream _stream;

    public AudioSourceType Type => _type;
    public string Filename => _filename;
    public Stream Stream => _stream;

    private AudioSource()
    {
    }

    public static AudioSource FromMp3(string filename)
    {
        return new()
        {
            _filename = filename,
            _type = AudioSourceType.Mp3File
        };
    }

    public static AudioSource FromMp3(Stream stream)
    {
        return new()
        {
            _stream = stream,
            _type = AudioSourceType.Mp3Stream
        };
    }

    public static AudioSource FromWave(string filename)
    {
        return new()
        {
            _filename = filename,
            _type = AudioSourceType.WaveFile
        };
    }

    public static AudioSource FromWave(Stream stream)
    {
        return new()
        {
            _stream = stream,
            _type = AudioSourceType.WaveStream
        };
    }

    internal StreamingSource CreateStreamingSource()
    {
        return _type switch
        {
            AudioSourceType.Mp3File => new MpegFileAdapter(_filename),
            AudioSourceType.WaveFile => new WavePcmSource(_filename),
            AudioSourceType.Mp3Stream => new MpegFileAdapter(_stream),
            AudioSourceType.WaveStream => new WavePcmSource(_stream),
            _ => null
        };
    }

    internal (byte[] pcm, AudioInfo info) CreateCompleteData()
    {
        if (_type == AudioSourceType.Mp3File)
        {
            using var mp3 = new MpegFile(_filename);
            int sampleRate = mp3.SampleRate;
            int channels = mp3.Channels;

            byte[] pcm = new byte[mp3.Length];
            int read = mp3.ReadSamplesInt16(pcm, 0, pcm.Length);

            return (pcm, new AudioInfo((float)mp3.Duration.TotalSeconds, sampleRate, 16, channels));
        }
        else if (_type == AudioSourceType.Mp3Stream)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using var mp3 = new MpegFile(_stream);
            int sampleRate = mp3.SampleRate;
            int channels = mp3.Channels;

            byte[] pcm = new byte[mp3.Length];
            int read = mp3.ReadSamplesInt16(pcm, 0, pcm.Length);

            return (pcm, new AudioInfo((float)mp3.Duration.TotalSeconds, sampleRate, 16, channels));
        }
        else if (_type == AudioSourceType.WaveFile)
        {
            using FileStream fs = File.OpenRead(_filename);
            WavPcmDecoder decoder = new();
            decoder.Load(fs);

            return (decoder.PcmData, new AudioInfo(decoder.Duration, decoder.SampleRate, decoder.BitsPerSample, decoder.Channels));
        }
        else if (_type == AudioSourceType.WaveStream)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            WavPcmDecoder decoder = new();
            decoder.Load(_stream);

            return (decoder.PcmData, new AudioInfo(decoder.Duration, decoder.SampleRate, decoder.BitsPerSample, decoder.Channels));
        }

        return default;
    }
}