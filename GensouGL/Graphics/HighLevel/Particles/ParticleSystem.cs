namespace GensouGL.Graphics.Particles;

public sealed class ParticleSystem
{
    private readonly List<Particle> _particles = new();
    private readonly List<ParticleEmitter> _emitters = new();
    private BatchBulletRenderer _renderer;
    private ITexture _globalTexture;
    private BlendMode _blendMode;

    public BatchBulletRenderer Renderer => _renderer;
    public ITexture GlobalTexture => _globalTexture;
    public BlendMode BlendMode
    {
        get => _blendMode;
        set => _blendMode = value;
    }

    public int ParticleCount => _particles.Count;
    public int EmitterCount => _emitters.Count;

    public ParticleSystem(BatchBulletRenderer renderer, ITexture globalTexture, BlendMode blendMode = BlendMode.Add)
    {
        _renderer = renderer;
        _globalTexture = globalTexture;
        _blendMode = blendMode;
    }

    public void Emit(ParticleEmitter emitter)
    {
        _emitters.Add(emitter);
    }

    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    public void Update(float deltaTime)
    {
        for (int i = _emitters.Count - 1; i >= 0; i--)
        {
            ParticleEmitter emitter = _emitters[i];
            bool finished = emitter.Update(this, deltaTime);
            if (finished)
            {
                _emitters.RemoveAt(i);
            }
        }

        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            Particle particle = _particles[i];
            particle.Update(deltaTime);
            if (particle.Life <= 0)
            {
                _particles.RemoveAt(i);
            }
        }
    }

    public void Render()
    {
        _renderer.SetBlendMode(_blendMode);
        _renderer.Begin(_globalTexture);
        for (int i = 0; i < _particles.Count; i++)
        {
            _renderer.Draw(_particles[i].Position.X, _particles[i].Position.Y, _particles[i].Size, _particles[i].Size, _particles[i].Rotation, _particles[i].GetCurrentColor(), _particles[i].Texture);
        }
        _renderer.End();
    }
}