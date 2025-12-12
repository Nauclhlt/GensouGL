namespace GensouGL.Graphics.Particles;

public sealed class Particle
{
    public GVector2 Position { get; set; }
    public GVector2 Velocity { get; set; }
    public GVector2 Accel { get; set; }
    public float Life { get; set; }
    public float TotalLife { get; set; }
    public float Size { get; set; }
    public float Rotation { get; set; }
    public float AngularVelocity { get; set; }
    public Color StartColor { get; set; }
    public Color EndColor { get; set; }

    public Color GetCurrentColor()
    {
        return GensouMathF.Lerp(StartColor, EndColor, (TotalLife - Life) / TotalLife);
    }

    public void Update(float deltaTime)
    {
        Velocity += Accel * deltaTime;
        Position += Velocity * deltaTime;

        Rotation += AngularVelocity;

        Life -= deltaTime;
    }
}