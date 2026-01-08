namespace GensouGL.GameDev;

/// <summary>
/// Manages a collection of string flags.
/// </summary>
public sealed class StringFlagCollection
{
    private readonly HashSet<string> _on;

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count => _on.Count;

    /// <summary>
    /// Initializes a new instance of empty <see cref="StringFlagCollection"/>.
    /// </summary>
    public StringFlagCollection()
    {
        _on = [];
    }

    /// <summary>
    /// Determines if the specified flag is on.
    /// </summary>
    /// <param name="flag">Target flag.</param>
    /// <returns>Flag.</returns>
    public bool GetFlag(string flag)
    {
        return _on.Contains(flag);
    }

    /// <summary>
    /// Turns the specified flag on.
    /// </summary>
    /// <param name="flag">Target flag.</param>
    /// <returns>Boolean representing whether the specified flag was off.</returns>
    public bool On(string flag)
    {
        if (_on.Add(flag))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Turns the specified flag off.
    /// </summary>
    /// <param name="flag">Target flag.</param>
    public void Off(string flag)
    {
        _on.Remove(flag);
    }

    /// <summary>
    /// Converts the current StringFlagCollection to string.
    /// </summary>
    /// <returns>Converted string.</returns>
    public override string ToString()
    {
        return $"[GensouGL.GameDev.StringFlagCollection with {Count} Flags on]";
    }
}