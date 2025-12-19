namespace GensouGL.Core.AsyncLoaders;

// For compatibility, streaming audios cannot support asynchronous loading.
// So in this implementation, Complete() runs everything synchronously.
public sealed class AsyncStreamingAudioLoader : IAsyncResourceLoadingProcess
{
    private AudioSource _source;
    private StreamingAudio _audio;

    public object Resource => _audio;

    public AsyncStreamingAudioLoader(AudioSource source)
    {
        _source = source;
    }

    public async Task Load()
    {
        
    }

    public void Complete()
    {
        _audio = StreamingAudio.From(_source);
    }
}