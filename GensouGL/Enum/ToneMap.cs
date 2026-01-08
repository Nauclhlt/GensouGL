namespace GensouGL.Graphics;

/// <summary>
/// Represents a tone map.
/// </summary>
public enum ToneMap
{
    /// <summary>
    /// No tone map.
    /// </summary>
    None = 0,
    /// <summary>
    /// Reinhard tone map.
    /// </summary>
    Reinhard = 1,
    /// <summary>
    /// ACES tone map.
    /// </summary>
    ACES = 2,
    /// <summary>
    /// Filmic tone map.
    /// </summary>
    Filmic = 3
}