namespace GensouGL.Graphics;

/// <summary>
/// Represents an ambient light.
/// </summary>
public readonly struct AmbientLight(Color color, float intensity)
{
    /// <summary>
    /// Color of the light.
    /// </summary>
    public readonly Color Color = color;
    /// <summary>
    /// Intensity of the light.
    /// </summary>
    public readonly float Intensity = intensity;
}