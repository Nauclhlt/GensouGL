namespace GensouGL.Graphics.Particles;

public abstract class ParticleEmitter
{
    protected ParticleSettings _settings;

    public abstract void Emit(Point point);
}