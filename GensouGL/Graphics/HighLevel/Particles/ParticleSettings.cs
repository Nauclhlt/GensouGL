namespace GensouGL.Graphics.Particles;

/// <summary>
/// Represents particle settings used by emitters.
/// </summary>
public sealed class ParticleSettings
{
    /// <summary>
    /// Represents a parameter that is determined randomly within a range.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    public sealed class RangeParameter<T>
    {
        /// <summary>
        /// Minimum value.
        /// </summary>
        public T Min { get; }
        /// <summary>
        /// Maximum value.
        /// </summary>
        public T Max { get; }

        /// <summary>
        /// Initializes a new instance of RangeParameter using the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        public RangeParameter(T min, T max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Implicitly converts the tuple into a RangeParameter.
        /// </summary>
        /// <param name="tuple">Source tuple.</param>
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

    /// <summary>
    /// Gets or sets the speed parameter.
    /// </summary>
    public RangeParameter<float> Speed
    {
        get => _speed;
        set => _speed = value;
    }

    /// <summary>
    /// Gets or sets the acceleration parameter.
    /// </summary>
    public RangeParameter<float> Accel
    {
        get => _accel;
        set => _accel = value;
    }

    /// <summary>
    /// Gets or sets the shoot angle parameter.
    /// </summary>
    public RangeParameter<float> ShootAngle
    {
        get => _shootAngle;
        set => _shootAngle = value;
    }

    /// <summary>
    /// Gets or sets the rotation angle parameter.
    /// </summary>
    public RangeParameter<float> Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    /// <summary>
    /// Gets or sets the angular velocity parameter in radians.
    /// </summary>
    public RangeParameter<float> AngularVelocity
    {
        get => _angularVelocity;
        set => _angularVelocity = value;
    }

    /// <summary>
    /// Gets or sets the life time parameter.
    /// </summary>
    public RangeParameter<float> Life
    {
        get => _life;
        set => _life = value;
    }

    /// <summary>
    /// Gets or sets the size parameter.
    /// </summary>
    public RangeParameter<float> Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// Gets or sets the start color parameter.
    /// </summary>
    public RangeParameter<Color> StartColor
    {
        get => _startColor;
        set => _startColor = value;
    }

    /// <summary>
    /// Gets or sets the end color parameter.
    /// </summary>
    public RangeParameter<Color> EndColor
    {
        get => _endColor;
        set => _endColor = value;
    }


    /// <summary>
    /// Gets or sets the texture.
    /// </summary>
    public ITexture Texture
    {
        get => _texture;
        set => _texture = value ?? GensouGL.Graphics.Texture.Empty;
    }

    /// <summary>
    /// Generates a speed within the specified range.
    /// </summary>
    /// <returns>Generated speed value.</returns>
    public float GenerateSpeed()
    {
        return Randomizer.NextFloat(Speed.Min, Speed.Max);
    }

    /// <summary>
    /// Generates a acceleration within the specified range.
    /// </summary>
    /// <returns>Generated acceleration value.</returns>
    public float GenerateAccel()
    {
        return Randomizer.NextFloat(Accel.Min, Accel.Max);
    }

    /// <summary>
    /// Generates a shoot angle within the specified range.
    /// </summary>
    /// <returns>Generated shoot angle value.</returns>
    public float GenerateShootAngle()
    {
        return Randomizer.NextFloat(ShootAngle.Min, ShootAngle.Max);
    }

    /// <summary>
    /// Generates a color within the specified range.
    /// </summary>
    /// <param name="source">Source range parameter.</param>
    /// <returns>Generated color.</returns>
    public Color GenerateColor(RangeParameter<Color> source)
    {
        return new Color(source.Min.R + (source.Max.R - source.Min.R) * Randomizer.NextFloat(),
                              source.Min.G + (source.Max.G - source.Min.G) * Randomizer.NextFloat(),
                              source.Min.B + (source.Max.B - source.Min.B) * Randomizer.NextFloat(),
                              source.Min.A + (source.Max.A - source.Min.A) * Randomizer.NextFloat());
    }

    /// <summary>
    /// Creates a particle based on the current settings.
    /// </summary>
    /// <returns>Created particle.</returns>
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