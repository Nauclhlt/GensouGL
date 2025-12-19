using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace GensouGL.Core;

internal static class AudioPlayer
{
    private static OpenALContext _context;

    internal static void Initialize()
    {
        _context = new();
    }

    internal static void Release()
    {
        _context.Dispose();
    }


    private static Dictionary<Guid, int> _streamingAudioSet = [];
    private static List<StreamingAudio> _streamingAudios = [];

    internal static void StreamingPlay(StreamingAudio audio)
    {
        if (_streamingAudioSet.ContainsKey(audio.UniqueId))
        {
#if DEBUG
            DebugLog.Error("AudioPlayer: Streaming audio has been already registered.");
#endif
            return;
        }

        AL.SourcePlay(audio.Source);

        _streamingAudioSet.Add(audio.UniqueId, _streamingAudios.Count);
        _streamingAudios.Add(audio);
    }

    internal static void StreamingStop(StreamingAudio audio)
    {
        AL.SourceStop(audio.Source);
        int pos = _streamingAudioSet[audio.UniqueId];
        _streamingAudioSet[_streamingAudios[^1].UniqueId] = pos;
        _streamingAudios[pos] = _streamingAudios[^1];
        _streamingAudios.RemoveAt(_streamingAudios.Count - 1);
        _streamingAudioSet.Remove(audio.UniqueId);
    }

    internal static void Update()
    {
        for (int i = 0; i < _streamingAudios.Count; i++)
        {
            _streamingAudios[i].Update();
        }
    }
}

internal sealed class OpenALContext : IDisposable
{
    private ALDevice _device;
    private ALContext _context;

    public ALDevice Device => _device;
    public ALContext Context => _context;

    public OpenALContext()
    {
        _device = ALC.OpenDevice(null);
        _context = ALC.CreateContext(_device, (int[])null);

        ALC.MakeContextCurrent(_context);
    }

    public void Dispose()
    {
        ALC.MakeContextCurrent(ALContext.Null);
        ALC.DestroyContext(_context);
        ALC.CloseDevice(_device);

        GC.SuppressFinalize(this);
    }

    ~OpenALContext()
    {
        Dispose();
    }
}