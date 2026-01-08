namespace GensouGL.Graphics;

/// <summary>
/// A post-processing effect that simulates light bleeding (glow) by extracting bright areas and blurring them.
/// </summary>
public sealed class BloomEffect : PostProcessEffect
{
    private float _threshold;
    private float _intensity;
    private float _knee;
    private float _spread;

    /// <summary>
    /// Gets or sets the luminance threshold. Pixels brighter than this value will contribute to the bloom.
    /// </summary>
    public float Threshold
    {
        get => _threshold;
        set => _threshold = value;
    }

    /// <summary>
    /// Gets or sets the strength of the bloom glow added back to the original image.
    /// </summary>
    public float Intensity
    {
        get => _intensity;
        set => _intensity = value;
    }

    /// <summary>
    /// Gets or sets the soft-knee value for a smoother transition between dark and bright areas.
    /// </summary>
    public float Knee
    {
        get => _knee;
        set => _knee = value;
    }

    /// <summary>
    /// Gets or sets the blur radius or distance for the light spread.
    /// </summary>
    public float Spread
    {
        get => _spread;
        set => _spread = value;
    }

    /// <summary>
    /// Gets a default instance of the bloom effect with standard parameters.
    /// </summary>
    public static BloomEffect Default { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BloomEffect"/> with default settings.
    /// </summary>
    public BloomEffect() : this(0.5f, 1.0f, 0.5f, 1.0f)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BloomEffect"/> with specific parameters.
    /// </summary>
    /// <param name="threshold">Luminance threshold.</param>
    /// <param name="intensity">Glow intensity.</param>
    /// <param name="knee">Soft-knee transition range.</param>
    /// <param name="spread">Blur spread distance.</param>
    public BloomEffect(float threshold, float intensity, float knee, float spread)
    {
        _threshold = threshold;
        _intensity = intensity;
        _knee = knee;
        _spread = spread;
    }

    internal override void Apply(Renderer renderer, Screen source, Screen dest)
    {
        Screen bright = TempScreenPool.Rent(source.Width, source.Height);
        Screen blur = TempScreenPool.Rent(source.Width, source.Height);

        bright.Clear(Color.Transparent);
        blur.Clear(Color.Transparent);

        blur.Texture.ResizeMode = TextureResizeMode.Bilinear;

        // Draw base
        QuickRenderer2D qr = GetSharedQuickRenderer(dest);
        qr.SetBlendMode(BlendMode.Normal);
        qr.DrawTexture(0, dest.Height, dest.Width, -dest.Height, source.Texture);

        // Extract bright pixels
        {
            Shader.BrightPass.SetUniformFloat("pThreshold", _threshold);
            Shader.BrightPass.SetUniformFloat("pKnee", _knee);

            Shader.BrightPass.Bind();

            FullscreenQuadRenderer.Draw(bright, source.Texture);
        }

        // Blur
        {
            Shader.BloomBlur.SetUniformFloat("pSpread", _spread);
            Shader.BloomBlur.SetUniformVector2("pDirection", 1.0f, 0.0f);

            Shader.BloomBlur.Bind();

            FullscreenQuadRenderer.Draw(blur, bright.Texture);

            Shader.BloomBlur.SetUniformFloat("pSpread", _spread);
            Shader.BloomBlur.SetUniformVector2("pDirection", 0.0f, 1.0f);
            Shader.BloomBlur.SetUniformFloat("pIntensity", _intensity);

            GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
            FullscreenQuadRenderer.Draw(dest, blur.Texture);
        }

        TempScreenPool.Return(bright);
        TempScreenPool.Return(blur);
    }
}