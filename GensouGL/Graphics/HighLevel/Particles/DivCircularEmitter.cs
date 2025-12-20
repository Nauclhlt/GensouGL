namespace GensouGL.Graphics.Particles;

public sealed class DivCircularEmitter : ParticleEmitter
{
    private float _interval;
    private float _wait;
    private int _emitCount;
    private int _particleCount;

    public DivCircularEmitter(ParticleSettings settings, GVector2 position, float interval, int emitCount, int particleCount)
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
            GVector2 dir = GVector2.UnitX;
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