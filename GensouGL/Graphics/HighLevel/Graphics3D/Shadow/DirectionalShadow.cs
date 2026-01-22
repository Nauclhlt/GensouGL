namespace GensouGL.Graphics;

/// <summary>
/// Represents directional shadow data.
/// </summary>
public readonly struct DirectionalShadow(Vec3 direction, float distance, float casterExtension, float depth)
{
    /// <summary>
    /// Gets the default <see cref="DirectionalShadow"/>.
    /// </summary>
    public static DirectionalShadow Default = new(-Vec3.UnitZ, 350f, 100f, 400f);

    /// <summary>
    /// Direction of the light.
    /// </summary>
    public readonly Vec3 Direction = direction;
    /// <summary>
    /// Distance of the light camera from the center.
    /// </summary>
    public readonly float Distance = distance;
    /// <summary>
    /// Caster extension value.
    /// </summary>
    public readonly float CasterExtension = casterExtension;
    /// <summary>
    /// Maximum depth of the shadow map.
    /// </summary>
    public readonly float Depth = depth;
}