namespace GensouGL.Graphics;

/// <summary>
/// Specifies font style mode.
/// </summary>
[Flags]
public enum FontStyleMode
{
    /// <summary>
    /// Regular style.
    /// </summary>
    Regular = 0,
    /// <summary>
    /// Bold style.
    /// </summary>
    Bold = 1,
    /// <summary>
    /// Italic style.
    /// </summary>
    Italic = 2,
    /// <summary>
    /// Bold and italic style.
    /// </summary>
    BoldItalic = 3
}