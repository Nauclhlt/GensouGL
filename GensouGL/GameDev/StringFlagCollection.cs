namespace GensouGL.GameDev;

public sealed class StringFlagCollection
{
    private readonly HashSet<string> _on;

    public int Count => _on.Count;

    public StringFlagCollection()
    {
        _on = [];
    }

    public bool GetFlag(string flag)
    {
        return _on.Contains(flag);
    }

    public bool On(string flag)
    {
        if (_on.Add(flag))
        {
            return true;
        }
        return false;
    }

    public void Off(string flag)
    {
        _on.Remove(flag);
    }

    public override string ToString()
    {
        return $"[GensouGL.GameDev.StringFlagCollection with {Count} Flags on]";
    }
}