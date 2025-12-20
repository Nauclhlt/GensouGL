namespace GensouGL.Graphics.Particles;

public abstract class ParticleEmitter
{
    protected ParticleSettings _settings;
    protected GVector2 _position;

    public abstract bool Update(ParticleSystem system, float deltaTime);
}