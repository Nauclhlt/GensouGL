namespace GensouGL.Graphics.Particles;

/// <summary>
/// Represents an abstract base class for particle emitters.
/// </summary>
/// <remarks>
/// To emit particles, create an instance of a emitter class derived from <see cref="ParticleEmitter"/>, then pass it to <see cref="ParticleSystem.Emit(GensouGL.Graphics.Particles.ParticleEmitter)"/>.
/// </remarks>
public abstract class ParticleEmitter
{
    /// <summary>
    /// Particle settings.
    /// </summary>
    protected ParticleSettings _settings;
    /// <summary>
    /// Emitter position.
    /// </summary>
    protected Vec2 _position;

    /// <summary>
    /// When overridden, updates the emitter state.
    /// </summary>
    /// <param name="system">Parent particle system.</param>
    /// <param name="deltaTime">Delta time.</param>
    /// <returns>True if all the emission processes are complete; otherwise, false.</returns>
    public abstract bool Update(ParticleSystem system, float deltaTime);
}