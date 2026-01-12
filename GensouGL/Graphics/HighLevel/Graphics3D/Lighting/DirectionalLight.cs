namespace GensouGL.Graphics;

/// <summary>
/// Represents a directional light.
/// </summary>
public readonly struct DirectionalLight(Color color, float intensity, Vec3 direction)
{
    /// <summary>
    /// Color of the light.
    /// </summary>
    public readonly Color Color = color;
    /// <summary>
    /// Intensity of the light.
    /// </summary>
    public readonly float Intensity = intensity;
    /// <summary>
    /// Normalized direction from which the light comes.
    /// </summary>
    public readonly Vec3 Direction = direction;
}