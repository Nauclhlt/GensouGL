using System.IO;
using System.Xml.Linq;
using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

internal sealed class WavePcmSource : StreamingSource
{
    private BinaryReader _reader;

    private int _channels;
    private int _sampleRate;
    private int _bitsPerSample;
    private float _duration;

    private long _pcmHead;
    private long _pcmLength;

    public WavePcmSource(string filename)
    {
        _reader = new(File.OpenRead(filename));
        Init();
    }

    public WavePcmSource(Stream stream)
    {
        _reader = new BinaryReader(stream);
        Init();
    }

    private void Init()
    {
        if (Encoding.ASCII.GetString(_reader.ReadBytes(4)) != "RIFF")
            throw new Exception("Invalid WAV: Missing RIFF");

        _reader.ReadInt32();
        if (Encoding.ASCII.GetString(_reader.ReadBytes(4)) != "WAVE")
            throw new Exception("Invalid WAV: Missing WAVE");

        bool fmtFound = false;
        bool dataFound = false;

        while (_reader.BaseStream.Position < _reader.BaseStream.Length)
        {
            string chunkId = Encoding.ASCII.GetString(_reader.ReadBytes(4));
            int chunkSize = _reader.ReadInt32();

            switch (chunkId)
            {
                case "fmt ":
                    int audioFormat = _reader.ReadInt16();
                    if (audioFormat != 1)
                        throw new Exception("Only PCM WAV is supported.");

                    _channels = _reader.ReadInt16();
                    _sampleRate = _reader.ReadInt32();

                    _reader.ReadInt32(); // byte rate
                    _reader.ReadInt16(); // block align

                    _bitsPerSample = _reader.ReadInt16();

                    if (chunkSize > 16)
                        _reader.BaseStream.Seek(chunkSize - 16, SeekOrigin.Current);

                    fmtFound = true;
                    break;

                case "data":
                    _pcmHead = _reader.BaseStream.Position;
                    _pcmLength = chunkSize;
                    _reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                    dataFound = true;
                    break;

                default:
                    _reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                    break;
            }

            if (fmtFound && dataFound) break;
        }

        if (!fmtFound) throw new Exception("WAV fmt chunk missing.");
        if (!dataFound) throw new Exception("WAV data chunk missing.");

        _duration = _pcmLength / (_sampleRate * _channels * (_bitsPerSample / 8.0f));

        _reader.BaseStream.Seek(_pcmHead, SeekOrigin.Begin);       
    }

    public override void GetInfo(out int sampleRate, out int bitsPerSample, out int channels, out float duration)
    {
        sampleRate = _sampleRate;
        bitsPerSample = _bitsPerSample;
        channels = _channels;
        duration = _duration;
    }

    public override int Read(byte[] buffer, int length)
    {
        int clamp = (int)long.Min(_pcmLength - _reader.BaseStream.Position + _pcmHead, length);
        if (clamp <= 0) return 0;

        _reader.Read(buffer, 0, clamp);
        return clamp;
    }

    public override void Seek(int position)
    {
        _reader.BaseStream.Seek(_pcmHead + position, SeekOrigin.Begin);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _reader?.Dispose();
            }

            _disposed = true;
        }
    }
}