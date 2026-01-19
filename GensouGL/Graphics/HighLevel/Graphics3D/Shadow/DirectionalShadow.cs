namespace GensouGL.Graphics;

/// <summary>
/// Represents directional shadow data.
/// </summary>
public readonly struct DirectionalShadow(Vec3 center, Vec3 direction)
{
    /// <summary>
    /// Gets the default <see cref="DirectionalShadow"/>.
    /// </summary>
    public static DirectionalShadow Default = new(Vec3.Zero, -Vec3.UnitZ);

    /// <summary>
    /// Center of the target area.
    /// </summary>
    public readonly Vec3 Center = center;
    /// <summary>
    /// Direction of the light.
    /// </summary>
    public readonly Vec3 Direction = direction;
}