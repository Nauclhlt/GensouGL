using System.IO;

namespace GensouGL.Audio;

/// <summary>
/// Represents a source of audio data, such as an MP3 or WAV file or stream.
/// </summary>
/// <remarks>Use the static factory methods to create an instance of this class from a file path or a stream
/// containing MP3 or WAV audio data. The type of audio source is determined by the factory method used. Once created,
/// the properties provide access to the underlying file name or stream, as well as the audio source type. This class is
/// immutable and thread-safe after construction.</remarks>
public sealed class AudioSource
{
    private AudioSourceType _type;
    private string _filename;
    private Stream _stream;

    /// <summary>
    /// Gets the type of the audio source.
    /// </summary>
    public AudioSourceType Type => _type;
    /// <summary>
    /// Gets the file name associated with this instance. Returns <see langword="null"/>, if the instance was not created from a file name.
    /// </summary>
    public string Filename => _filename;
    /// <summary>
    /// Gets the stream associated with this instance. Returns <see langword="null"/>, if the instance was not created from a stream.
    /// </summary>
    public Stream Stream => _stream;

    private AudioSource()
    {
    }

    /// <summary>
    /// Creates a new AudioSource instance that contains the specified MP3 file.
    /// </summary>
    /// <param name="filename">The path to the MP3 file to use as the audio source. Cannot be null or empty.</param>
    /// <returns>An AudioSource configured to contain the specified MP3 file.</returns>
    public static AudioSource FromMp3(string filename)
    {
        return new()
        {
            _filename = filename,
            _type = AudioSourceType.Mp3File
        };
    }

    /// <summary>
    /// Creates a new <see cref="AudioSource"/> instance that reads audio data from the specified MP3 stream.
    /// </summary>
    /// <remarks>The caller is responsible for managing the lifetime of the provided stream. The stream must
    /// remain open and readable for the duration of the <see cref="AudioSource"/>'s use.</remarks>
    /// <param name="stream">The stream containing MP3 audio data. The stream must be readable and positioned at the beginning of the MP3
    /// content.</param>
    /// <returns>An <see cref="AudioSource"/> that provides audio data from the specified MP3 stream.</returns>
    public static AudioSource FromMp3(Stream stream)
    {
        return new()
        {
            _stream = stream,
            _type = AudioSourceType.Mp3Stream
        };
    }

    /// <summary>
    /// Creates a new AudioSource instance that contains the specified Ogg file.
    /// </summary>
    /// <param name="filename">The path to the Ogg file to use as the audio source. Cannot be null or empty.</param>
    /// <returns>An AudioSource configured to contain the specified Ogg file.</returns>
    public static AudioSource FromOgg(string filename)
    {
        return new()
        {
            _filename = filename,
            _type = AudioSourceType.OggFile
        };
    }

    /// <summary>
    /// Creates a new <see cref="AudioSource"/> instance that reads audio data from the specified Ogg stream.
    /// </summary>
    /// <remarks>The caller is responsible for managing the lifetime of the provided stream. The stream must
    /// remain open and readable for the duration of the <see cref="AudioSource"/>'s use.</remarks>
    /// <param name="stream">The stream containing Ogg audio data. The stream must be readable and positioned at the beginning of the Ogg
    /// content.</param>
    /// <returns>An <see cref="AudioSource"/> that provides audio data from the specified Ogg stream.</returns>
    public static AudioSource FromOgg(Stream stream)
    {
        return new()
        {
            _stream = stream,
            _type = AudioSourceType.OggStream
        };
    }

    /// <summary>
    /// Creates a new <see cref="AudioSource"/> instance that contains audio from the specified Wave file.
    /// </summary>
    /// <param name="filename">The path to the Wave (.wav) file to use as the audio source. Cannot be null or empty.</param>
    /// <returns>An <see cref="AudioSource"/> configured to contain the specified Wave file.</returns>
    public static AudioSource FromWave(string filename)
    {
        return new()
        {
            _filename = filename,
            _type = AudioSourceType.WaveFile
        };
    }

    /// <summary>
    /// Creates a new <see cref="AudioSource"/> instance that reads audio data from the specified Wave stream.
    /// </summary>
    /// <remarks>The caller is responsible for managing the lifetime of the provided stream. The stream must
    /// remain open and readable for the duration of the <see cref="AudioSource"/>'s use.</remarks>
    /// <param name="stream">The stream containing Wave audio data. The stream must be readable and positioned at the beginning of the Wave
    /// content.</param>
    /// <returns>An <see cref="AudioSource"/> that provides audio data from the specified Wave stream.</returns>

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
            AudioSourceType.WaveFile => new WavePcmSource(_filename),
            AudioSourceType.WaveStream => new WavePcmSource(_stream),
            AudioSourceType.OggFile => new OggVorbisAdapter(_filename),
            AudioSourceType.OggStream => new OggVorbisAdapter(_stream),
            _ => null
        };
    }

    internal (byte[] pcm, AudioInfo info) CreateCompleteData()
    {
        if (_type == AudioSourceType.Mp3File)
        {
            using var stream = GensouFileSystem.OpenReadFile(_filename);
            using var mp3 = new MpegFile(stream);
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
        else if (_type == AudioSourceType.OggFile)
        {
            return OggDecoder.Load(_filename);
        }
        else if (_type == AudioSourceType.OggStream)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            return OggDecoder.Load(_stream, false);
        }

        return default;
    }
}