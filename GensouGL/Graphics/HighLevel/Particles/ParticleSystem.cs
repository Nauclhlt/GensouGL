namespace GensouGL.Graphics.Particles;

/// <summary>
/// Manages the collection, life cycle, and rendering of particles and emitters.
/// </summary>
public sealed class ParticleSystem
{
    private readonly List<Particle> _particles = new();
    private readonly List<ParticleEmitter> _emitters = new();
    private BatchBulletRenderer2D _renderer;
    private ITexture _globalTexture;
    private BlendMode _blendMode;

    /// <summary>
    /// Gets or sets the renderer used to draw particles.
    /// </summary>
    public BatchBulletRenderer2D Renderer => _renderer;
    /// <summary>
    /// Gets or sets the global texture.
    /// </summary>
    public ITexture GlobalTexture => _globalTexture;
    /// <summary>
    /// Gets or sets the blend mode used to render particles.
    /// </summary>
    public BlendMode BlendMode
    {
        get => _blendMode;
        set => _blendMode = value;
    }

    /// <summary>
    /// Gets the number of active particles.
    /// </summary>
    public int ParticleCount => _particles.Count;
    /// <summary>
    /// Gets the number of active emitters.
    /// </summary>
    public int EmitterCount => _emitters.Count;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleSystem" /> using the specified parameters.
    /// </summary>
    /// <param name="renderer">Batch renderer for particles.</param>
    /// <param name="globalTexture">Default texture to use.</param>
    /// <param name="blendMode">Rendering blend mode (defaults to Additive).</param>
    public ParticleSystem(BatchBulletRenderer2D renderer, ITexture globalTexture, BlendMode blendMode = BlendMode.Add)
    {
        _renderer = renderer;
        _globalTexture = globalTexture;
        _blendMode = blendMode;
    }

    /// <summary>
    /// Registers an emitter to the system to start generating particles.
    /// </summary>
    /// <param name="emitter">Emitter to add.</param>
    public void Emit(ParticleEmitter emitter)
    {
        _emitters.Add(emitter);
    }

    /// <summary>
    /// Manually adds a single particle to the system.
    /// </summary>
    /// <param name="particle">Particle to add.</param>
    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    /// <summary>
    /// Updates all active emitters and particles, removing those that have finished or expired.
    /// </summary>
    /// <param name="deltaTime">Elapsed time since the last update.</param>
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

    /// <summary>
    /// Renders all active particles using the assigned <see cref="BatchBulletRenderer2D"/>.
    /// </summary>
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