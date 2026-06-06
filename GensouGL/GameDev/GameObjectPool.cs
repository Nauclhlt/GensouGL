namespace GensouGL.GameDev;

/// <summary>
/// Manages pooled game objects.
/// </summary>
public sealed class GameObjectPool
{
    /// <summary>
    /// Initializes a new instance of <see cref="GameObjectPool"/>.
    /// </summary>
    public GameObjectPool()
    {
        _pools = [];
    }

    Dictionary<IGameObjectPoolData, Queue<object>> _pools;

    /// <summary>
    /// Registers a new game object pool.
    /// </summary>
    /// <param name="data"><see cref="IGameObjectPoolData"/> to register.</param>
    public void Register(IGameObjectPoolData data)
    {
        _pools[data] = new(data.PoolSize);
        for (int i = 0; i < data.PoolSize; i++)
        {
            object instance = data.Instantiate();
            data.Disable(instance);
            _pools[data].Enqueue(instance);
        }
    }

    /// <summary>
    /// Gets an instance of the game object.
    /// </summary>
    /// <typeparam name="T">Type of the game object.</typeparam>
    /// <param name="data">Target <see cref="IGameObjectPoolData"/>.</param>
    /// <returns>Instance of the game object.</returns>
    public T Get<T>(IGameObjectPoolData data)
    {
        if (_pools.TryGetValue(data, out Queue<object> pool))
        {
            if (pool.Count > 0)
            {
                object instance = pool.Dequeue();
                data.Enable(instance);
                return (T)instance;
            }
            else
            {
                object instance = data.Instantiate();
                data.Enable(instance);
                return (T)instance;
            }
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Returns the instance.
    /// </summary>
    /// <param name="data">Target <see cref="IGameObjectPoolData"/>.</param>
    /// <param name="instance">Instance to return.</param>
    public void Return(IGameObjectPoolData data, object instance)
    {
        Type type = instance.GetType();
        if (_pools.TryGetValue(data, out Queue<object> pool))
        {
            data.Disable(instance);
            pool.Enqueue(type);
        }
    }
}