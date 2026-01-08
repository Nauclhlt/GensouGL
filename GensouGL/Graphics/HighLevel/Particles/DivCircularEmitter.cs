namespace GensouGL.Graphics.Particles;

/// <summary>
/// An emitter that releases particles in a circular, evenly distributed pattern.
/// </summary>
public sealed class DivCircularEmitter : ParticleEmitter
{
    private float _interval;
    private float _wait;
    private int _emitCount;
    private int _particleCount;

    /// <summary>
    /// Initializes a new instance of <see cref="DivCircularEmitter"/>.
    /// </summary>
    /// <param name="settings">The settings for the particles to be created.</param>
    /// <param name="position">The central position of the emission.</param>
    /// <param name="interval">The time interval between emissions (in seconds).</param>
    /// <param name="emitCount">The total number of emission cycles.</param>
    /// <param name="particleCount">The number of particles to release per cycle.</param>
    public DivCircularEmitter(ParticleSettings settings, Vec2 position, float interval, int emitCount, int particleCount)
    {
        _settings = settings;
        _position = position;
        _interval = interval;
        _wait = 0f;
        _emitCount = emitCount;
        _particleCount = particleCount;
    }

    /// <summary>
    /// Updates the emitter state and releases particles if the interval has elapsed.
    /// </summary>
    /// <param name="system">The particle system to add particles to.</param>
    /// <param name="deltaTime">The elapsed time since the last update.</param>
    /// <returns>True if all emission cycles are complete; otherwise, false.</returns>
    public override bool Update(ParticleSystem system, float deltaTime)
    {
        _wait -= deltaTime;
        if (_wait <= 0f)
        {
            Vec2 dir = Vec2.UnitX;
            float deltaAngle = MathF.PI * 2f / _particleCount;

            for (int i = 0; i < _particleCount; i++)
            {
                Particle p = _settings.CreateParticle();
                p.Position = _position;
                p.Velocity = dir * _settings.GenerateSpeed();
                p.Accel = dir * _settings.GenerateAccel();
                system.AddParticle(p);
                dir = dir.Rotate(deltaAngle);
            }
            _wait += _interval;
            _emitCount--;
        }

        return _emitCount <= 0;
    }
}