namespace GensouGL.GameDev;

/// <summary>
/// Provides data for pooled game objects.
/// </summary>
public interface IGameObjectPoolData
{
    /// <summary>
    /// Instantiates a new instance of the game object.
    /// </summary>
    /// <returns></returns>
    public object Instantiate();
    /// <summary>
    /// Enables the instance.
    /// </summary>
    /// <param name="instance">Instance to be enabled.</param>
    public void Enable(object instance);
    /// <summary>
    /// Disables the instance.
    /// </summary>
    /// <param name="instance">Instance to be disabled.</param>
    public void Disable(object instance);

    /// <summary>
    /// Gets the size of the pool.
    /// </summary>
    public int PoolSize { get; }
}