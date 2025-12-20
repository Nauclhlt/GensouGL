using System.Collections.Concurrent;
namespace GensouGL.Core;

public sealed class AsyncResourceLoader : IDisposable
{
    private int _loadedCount = 0;
    private int _totalCount = 0;
    private int _workerCount = 1;
    private bool _disposed = false;
    private ConcurrentQueue<(string, IAsyncResourceLoadingProcess)> _loadQueue = [];
    private ConcurrentQueue<(string, IAsyncResourceLoadingProcess)> _completeQueue = [];
    private SemaphoreSlim _semaphore = new(0);
    private bool _started = false;
    private Dictionary<string, object> _map = new();

    public int LoadedCount => _loadedCount;
    public int TotalCount => _totalCount;
    public int WorkerCount => _workerCount;
    public bool IsReady => _totalCount == _loadedCount;
    public bool Disposed => _disposed;
    public bool Started => _started;

    public AsyncResourceLoader(int workerCount = 1)
    {
        _workerCount = int.Clamp(workerCount, 1, int.Max(1, Environment.ProcessorCount - 1));
    }

    private void CheckState()
    {
        if (!_started)
        {
            _started = true;
            for (int i = 0; i < _workerCount; i++)
            {
                Task.Run(Worker);
            }
        }
    }

    private async void Worker()
    {
        while (_started)
        {
            await _semaphore.WaitAsync();

            if (_loadQueue.TryDequeue(out var proc))
            {
                await proc.Item2.Load();

                _completeQueue.Enqueue(proc);
            }
        }
    }

    public void Stop()
    {
        _started = false;
    }

    public void ProcessQueue()
    {
        if (_completeQueue.TryDequeue(out var proc))
        {
            proc.Item2.Complete();
            _loadedCount++;
            _map[proc.Item1] = proc.Item2.Resource;
            DebugLog.Info($"AsyncResourceLoader: Loaded '{proc.Item1}'");
        }
    }

    public void QueueTexture(string name, TextureSource source)
    {
        QueueCore(name, new AsyncTextureLoader(source));
    }

    public void QueueStreamingAudio(string name, AudioSource source)
    {
        QueueCore(name, new AsyncStreamingAudioLoader(source));
    }

    public void QueueMemoryAudio(string name, AudioSource source)
    {
        QueueCore(name, new AsyncMemoryAudioLoader(source));
    }

    public void QueueText(string name, string filename)
    {
        QueueCore(name, new AsyncTextLoader(filename));
    }

    private void QueueCore(string name, IAsyncResourceLoadingProcess proc)
    {
        if (_map.ContainsKey(name))
        {
#if DEBUG
            DebugLog.Error($"AsyncResourceLoader: '{name}' has been already loaded or is in the queue.");
#endif
            return;
        }
        _map[name] = null; 
        CheckState();
        _loadQueue.Enqueue((name, proc));
        _totalCount++;
        _semaphore.Release();
    }


    public T Get<T>(string name)
    {
        if (_map.TryGetValue(name, out var result))
        {
            return (T)result;
        }
        else
        {
            return default;
        }
    }
    

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                foreach (object item in _map.Values)
                {
                    if (item is IDisposable dp)
                        dp.Dispose();
                }
            }

            _started = false;

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~AsyncResourceLoader()
    {
        Dispose(false);
    }
}