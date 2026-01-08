namespace GensouGL.Graphics.Particles;

/// <summary>
/// Represents an individual particle with physical properties and life cycle management.
/// </summary>
public sealed class Particle
{
    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    public Vec2 Position { get; set; }
    /// <summary>
    /// Gets or sets the velocity.
    /// </summary>
    public Vec2 Velocity { get; set; }
    /// <summary>
    /// Gets or sets the acceleration.
    /// </summary>
    public Vec2 Accel { get; set; }
    /// <summary>
    /// Gets or sets the remaining life time in seconds.
    /// </summary>
    public float Life { get; set; }
    /// <summary>
    /// Gets or sets the total life time in seconds.
    /// </summary>
    public float TotalLife { get; set; }
    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    public float Size { get; set; }
    /// <summary>
    /// Gets or sets the rotation angle in radians.
    /// </summary>
    public float Rotation { get; set; }
    /// <summary>
    /// Gets or sets the angular velocity.
    /// </summary>
    public float AngularVelocity { get; set; }
    /// <summary>
    /// Gets or sets the color at the start.
    /// </summary>
    public Color StartColor { get; set; }
    /// <summary>
    /// Gets or sets the color at the end.
    /// </summary>
    public Color EndColor { get; set; }
    /// <summary>
    /// Gets or sets the texture.
    /// </summary>
    public ITexture Texture { get; set; }

    /// <summary>
    /// Calculates the current color based on the ratio of elapsed life.
    /// </summary>
    /// <returns>Linearly interpolated color between <see cref="StartColor"/> and <see cref="EndColor"/>.</returns>
    public Color GetCurrentColor()
    {
        return GensouMathF.Lerp(StartColor, EndColor, (TotalLife - Life) / TotalLife);
    }

    /// <summary>
    /// Updates the particle's physical state and life time based on elapsed time.
    /// </summary>
    /// <param name="deltaTime">Elapsed time since the last update.</param>
    public void Update(float deltaTime)
    {
        Velocity += Accel * deltaTime;
        Position += Velocity * deltaTime;

        Rotation += AngularVelocity * deltaTime;

        Life -= deltaTime;
    }
}