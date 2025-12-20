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

        public static implicit operator RangeParameter<T>((T min, T max) tuple)
        {
            return new RangeParameter<T>(tuple.min, tuple.max);
        }
    }

    private RangeParameter<float> _speed = new(16f, 16f);
    private RangeParameter<float> _accel = new(0f, 0f);
    private RangeParameter<float> _shootAngle = new(0f, 0f);
    private RangeParameter<float> _rotation = new(0f, GensouMathF.Rad(180));
    private RangeParameter<float> _angularVelocity = new(0f, 0f);
    private RangeParameter<float> _life = new(1f, 1.5f);
    private RangeParameter<float> _size = new(8f, 16f);
    private RangeParameter<Color> _startColor = new(Color.White, Color.White);
    private RangeParameter<Color> _endColor = new(Color.White, Color.White);
    private ITexture _texture = GensouGL.Graphics.Texture.Empty;

    public RangeParameter<float> Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public RangeParameter<float> Accel
    {
        get => _accel;
        set => _accel = value;
    }
    public RangeParameter<float> ShootAngle
    {
        get => _shootAngle;
        set => _shootAngle = value;
    }
    public RangeParameter<float> Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    public RangeParameter<float> AngularVelocity
    {
        get => _angularVelocity;
        set => _angularVelocity = value;
    }
    public RangeParameter<float> Life
    {
        get => _life;
        set => _life = value;
    }
    public RangeParameter<float> Size
    {
        get => _size;
        set => _size = value;
    }
    public RangeParameter<Color> StartColor
    {
        get => _startColor;
        set => _startColor = value;
    }
    public RangeParameter<Color> EndColor
    {
        get => _endColor;
        set => _endColor = value;
    }
    public ITexture Texture
    {
        get => _texture;
        set => _texture = value ?? GensouGL.Graphics.Texture.Empty;
    }

    public float GenerateSpeed()
    {
        return Randomizer.NextFloat(Speed.Min, Speed.Max);
    }

    public float GenerateAccel()
    {
        return Randomizer.NextFloat(Accel.Min, Accel.Max);
    }

    public float GenerateShootAngle()
    {
        return Randomizer.NextFloat(ShootAngle.Min, ShootAngle.Max);
    }

    public Color GenerateColor(RangeParameter<Color> source)
    {
        return Color.FromFloat(source.Min.Rf + (source.Max.Rf - source.Min.Rf) * Randomizer.NextFloat(),
                              source.Min.Gf + (source.Max.Gf - source.Min.Gf) * Randomizer.NextFloat(),
                              source.Min.Bf + (source.Max.Bf - source.Min.Bf) * Randomizer.NextFloat(),
                              source.Min.Af + (source.Max.Af - source.Min.Af) * Randomizer.NextFloat());
    }

    public Particle CreateParticle()
    {
        Particle particle = new()
        {
            TotalLife = Randomizer.NextFloat(Life.Min, Life.Max),
            Size = Randomizer.NextFloat(Size.Min, Size.Max),
            Rotation = Randomizer.NextFloat(Rotation.Min, Rotation.Max),
            AngularVelocity = Randomizer.NextFloat(AngularVelocity.Min, AngularVelocity.Max),
            StartColor = GenerateColor(_startColor),
            EndColor = GenerateColor(_endColor),
            Texture = this.Texture
        };
        particle.Life = particle.TotalLife;

        return particle;
    }
}