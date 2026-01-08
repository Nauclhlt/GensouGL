namespace GensouGL.GameDev;

/// <summary>
/// Manages integer counts associated with string keys.
/// </summary>
public sealed class KeyCounter
{
    private Dictionary<string, int> _map;

    /// <summary>
    /// Initializes a new instance of <see cref="KeyCounter" />.
    /// </summary>
    public KeyCounter()
    {
        _map = new Dictionary<string, int>();
    }

    /// <summary>
    /// Increases the count associated with the specified key by 1.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Count after increment.</returns>
    public int Increase(string key)
    {
        if (_map.ContainsKey(key))
        {
            _map[key]++;
        }
        else
        {
            _map[key] = 1;
        }
        return _map[key];
    }

    /// <summary>
    /// Increases the count associated with the specified key by the specified amount.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <param name="count">Count.</param>
    /// <returns>Count after increment.</returns>
    public int Increase(string key, int count)
    {
        if (_map.ContainsKey(key))
        {
            _map[key] += count;
        }
        else
        {
            _map[key] = count;
        }
        return _map[key];
    }

    /// <summary>
    /// Gets the count associated with the specified key.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Count associated with the specified key.</returns>
    public int CountOf(string key)
    {
        return _map.TryGetValue(key, out int count) ? count : 0;
    }

    /// <summary>
    /// Decreases the count associated with the specified key by 1.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Count after decrement.</returns>
    public int Decrease(string key)
    {
        if (_map.ContainsKey(key))
        {
            _map[key]--;
            if (_map[key] <= 0)
            {
                _map.Remove(key);
                return 0;
            }
            else
            {
                return _map[key];
            }
        }
        return 0;
    }

    /// <summary>
    /// Decreases the count associated with the specified key by the specified amount.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <param name="count">Count.</param>
    /// <returns>Count after decrement.</returns>
    public int Remove(string key, int count)
    {
        if (_map.ContainsKey(key))
        {
            _map[key] -= count;
            if (_map[key] <= 0)
            {
                _map.Remove(key);
                return 0;
            }
            else
            {
                return _map[key];
            }
        }
        return 0;
    }

    /// <summary>
    /// Returns if the specified key is contained in the key counter.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Flag representing whether the specified key is contained in the key counter.</returns>
    public bool Contains(string key)
    {
        return _map.ContainsKey(key);
    }

    /// <summary>
    /// Clears all keys.
    /// </summary>
    public void Clear()
    {
        _map.Clear();
    }
}