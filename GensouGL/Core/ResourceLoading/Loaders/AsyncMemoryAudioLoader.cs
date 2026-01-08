namespace GensouGL.Core.AsyncLoaders;

// For compatibility, memory audios cannot support asynchronous loading.
// So in this implementation, Complete() runs everything synchronously.
internal sealed class AsyncMemoryAudioLoader : IAsyncResourceLoadingProcess
{
    private AudioSource _source;
    private MemoryAudio _audio;

    public object Resource => _audio;

    public AsyncMemoryAudioLoader(AudioSource source)
    {
        _source = source;
    }

    public async Task Load()
    {

    }

    public void Complete()
    {
        _audio = MemoryAudio.From(_source);
    }
}