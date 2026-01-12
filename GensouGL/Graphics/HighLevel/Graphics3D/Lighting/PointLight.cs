namespace GensouGL.Graphics;

/// <summary>
/// Represents a point light.
/// </summary>
public readonly struct PointLight(Vec3 position, Color color, float intensity, float range)
{
    /// <summary>
    /// Position of the light.
    /// </summary>
    public readonly Vec3 Position = position;
    /// <summary>
    /// Color of the light.
    /// </summary>
    public readonly Color Color = color;
    /// <summary>
    /// Intensity of the light.
    /// </summary>
    public readonly float Intensity = intensity;
    /// <summary>
    /// Effective range of the light.
    /// </summary>
    public readonly float Range = range;
}