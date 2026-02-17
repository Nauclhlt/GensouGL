using System.Collections.Concurrent;
namespace GensouGL.Core;

/// <summary>
/// Provides asynchronous resource loading.
/// </summary>
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

    /// <summary>
    /// Gets the total number of resources loaded.
    /// </summary>
    public int LoadedCount => _loadedCount;
    /// <summary>
    /// Gets the total number of resources requested.
    /// </summary>
    public int TotalCount => _totalCount;
    /// <summary>
    /// Gets the number of workers.
    /// </summary>
    public int WorkerCount => _workerCount;
    /// <summary>
    /// Gets whether all resources has been loaded.
    /// </summary>
    public bool IsReady => _totalCount == _loadedCount;
    /// <summary>
    /// Gets whethter the instance is disposed.
    /// </summary>
    public bool Disposed => _disposed;
    /// <summary>
    /// Gets whether the loading process has been started.
    /// </summary>
    public bool Started => _started;

    /// <summary>
    /// Initializes a new instance of <see cref="AsyncResourceLoader"/>.
    /// </summary>
    /// <param name="workerCount">Default value is 1. Value representing the number of workers to be used to load resources.</param>
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

    /// <summary>
    /// Stops the resource loading process.
    /// </summary>
    public void Stop()
    {
        _started = false;
    }

    /// <summary>
    /// Dequeues and complete one loading process from the queue, if any.
    /// </summary>
    public void ProcessQueue()
    {
        if (_completeQueue.TryDequeue(out var proc))
        {
            proc.Item2.Complete();
            _loadedCount++;
            _map[proc.Item1] = proc.Item2.Resource;
            DebugLog.Info($"{nameof(AsyncResourceLoader)}: Loaded '{proc.Item1}'");
        }
    }

    /// <summary>
    /// Queues a new loading process of the texture.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="source"><see cref="TextureSource"/> from which the texture should be loaded.</param>
    public void QueueTexture(string name, TextureSource source)
    {
        QueueCore(name, new AsyncTextureLoader(source));
    }

    /// <summary>
    /// Queues a new loading process of the streaming audio.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="source"><see cref="AudioSource"/> from which the texture should be loaded.</param>
    public void QueueStreamingAudio(string name, AudioSource source)
    {
        QueueCore(name, new AsyncStreamingAudioLoader(source));
    }

    /// <summary>
    /// Queues a new loading process of the memory audio.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="source"><see cref="AudioSource"/> from which the texture should be loaded.</param>
    public void QueueMemoryAudio(string name, AudioSource source)
    {
        QueueCore(name, new AsyncMemoryAudioLoader(source));
    }

    /// <summary>
    /// Queues a new loading process of the text file.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="filename">Source file name.</param>
    public void QueueText(string name, string filename)
    {
        QueueCore(name, new AsyncTextLoader(filename));
    }

    /// <summary>
    /// Queues a new loading process of the static 3D model.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="source">Source file name.</param>
    public void QueueStaticModel3D(string name, string source)
    {
        QueueCore(name, new AsyncStaticModelLoader(source));
    }

    private void QueueCore(string name, IAsyncResourceLoadingProcess proc)
    {
        if (_map.ContainsKey(name))
        {
            DebugLog.Error($"{nameof(AsyncResourceLoader)}: '{name}' has been already loaded or is in the queue.");
            return;
        }
        _map[name] = null; 
        CheckState();
        _loadQueue.Enqueue((name, proc));
        _totalCount++;
        _semaphore.Release();
    }

    /// <summary>
    /// Returns the resource with the specified name as T.
    /// </summary>
    /// <typeparam name="T">Type of the target resource.</typeparam>
    /// <param name="name">Target resource name.</param>
    /// <returns>Target resource as T</returns>
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

    /// <summary>
    /// Releases all resources contained by <see cref="AsyncResourceLoader"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="AsyncResourceLoader"/> destructor.
    /// </summary>
    ~AsyncResourceLoader()
    {
        Dispose(false);
    }
}