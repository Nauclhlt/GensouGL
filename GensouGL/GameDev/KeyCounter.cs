namespace GensouGL.GameDev;

public sealed class KeyCounter
{
    private Dictionary<string, int> _map;

    public KeyCounter()
    {
        _map = new Dictionary<string, int>();
    }

    public int Add(string key)
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

    public int Add(string key, int count)
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

    public int CountOf(string key)
    {
        return _map.TryGetValue(key, out int count) ? count : 0;
    }

    public int Remove(string key)
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

    public bool Contains(string key)
    {
        return _map.ContainsKey(key);
    }

    public void Clear()
    {
        _map.Clear();
    }
}