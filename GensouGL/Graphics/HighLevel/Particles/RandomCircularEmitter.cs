namespace GensouGL.Graphics.Particles;

public sealed class RandomCircularEmitter : ParticleEmitter
{
    private float _interval;
    private float _wait;
    private int _emitCount;
    private int _particleCount;

    public RandomCircularEmitter(ParticleSettings settings, GVector2 position, float interval, int emitCount, int particleCount)
    {
        _settings = settings;
        _position = position;
        _interval = interval;
        _wait = 0f;
        _emitCount = emitCount;
        _particleCount = particleCount;
    }

    public override bool Update(ParticleSystem system, float deltaTime)
    {
        _wait -= deltaTime;
        if (_wait <= 0f)
        {
            for (int i = 0; i < _particleCount; i++)
            {
                GVector2 dir = GVector2.UnitX.Rotate(_settings.GenerateShootAngle());
                Particle p = _settings.CreateParticle();
                p.Position = _position;
                p.Velocity = dir * _settings.GenerateSpeed();
                p.Accel = dir * _settings.GenerateAccel();
                system.AddParticle(p);
            }
            _wait += _interval;
            _emitCount--;
        }

        return _emitCount <= 0;
    }
}