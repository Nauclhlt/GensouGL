namespace GensouGL.Graphics.Particles;

public sealed class ParticleSettings
{
    public sealed class RangeParameter<T>
    {
        public T Min { get; }
        public T Max { get; }

        public RangeParameter(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }

    private RangeParameter<float> _speed;
    private RangeParameter<float> _accel;
    private RangeParameter<float> _shootAngle;
    private RangeParameter<float> _rotation;
    private RangeParameter<float> _angularVelocity;
    private RangeParameter<float> _life;
    private RangeParameter<float> _size;
    private Color _startColor;
    private Color _endColor;

    public RangeParameter<float> Speed => _speed;
    public RangeParameter<float> Accel => _accel;
    public RangeParameter<float> ShootAngle => _shootAngle;
    public RangeParameter<float> Rotation => _rotation;
    public RangeParameter<float> AngularVelocity => _angularVelocity;
    public RangeParameter<float> Life => _life;
    public RangeParameter<float> Size => _size;
    public Color StartColor
    {
        get => _startColor;
        set => _startColor = value;
    }
    public Color EndColor
    {
        get => _endColor;
        set => _endColor = value;
    }

    public Particle CreateParticle()
    {
        Particle particle = new()
        {
            TotalLife = Randomizer.NextFloat(Life.Min, Life.Max),
            Size = Randomizer.NextFloat(Size.Min, Size.Max),
            Rotation = Randomizer.NextFloat(Rotation.Min, Rotation.Max),
            AngularVelocity = Randomizer.NextFloat(AngularVelocity.Min, AngularVelocity.Max),
            StartColor = StartColor,
            EndColor = EndColor
        };
        particle.Life = particle.TotalLife;
        

        return particle;
    }
}