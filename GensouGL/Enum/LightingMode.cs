namespace GensouGL.Graphics;

/// <summary>
/// Specifies the lighting mode.
/// </summary>
public enum LightingMode
{
    /// <summary>
    /// No lighting.
    /// </summary>
    NoLighting,
    /// <summary>
    /// Runs lighting.
    /// </summary>
    Lighting,
    /// <summary>
    /// Runs lighting and shadow mapping.
    /// </summary>
    LightingWithShadows
}