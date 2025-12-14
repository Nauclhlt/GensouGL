namespace GensouGL.Graphics;

public sealed class BloomEffect : PostProcessEffect
{
    private float _threshold;
    private float _intensity;
    private float _knee;
    private float _spread;

    public float Threshold
    {
        get => _threshold;
        set => _threshold = value;
    }

    public float Intensity
    {
        get => _intensity;
        set => _intensity = value;
    }

    public float Knee
    {
        get => _knee;
        set => _knee = value;
    }

    public float Spread
    {
        get => _spread;
        set => _spread = value;
    }

    public BloomEffect() : this(0.5f, 1.0f, 0.5f, 1.0f)
    {
    }

    public BloomEffect(float threshold, float intensity, float knee, float spread)
    {
        _threshold = threshold;
        _intensity = intensity;
        _knee = knee;
        _spread = spread;
    }

    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        Screen bright = TempScreenPool.Rent(source.Width, source.Height);
        Screen blur = TempScreenPool.Rent(source.Width, source.Height);

        bright.Clear(Color.Transparent);
        blur.Clear(Color.Transparent);

        blur.Texture.ResizeMode = TextureResizeMode.Bilinear;

        // Draw base
        QuickRenderer qr = GetSharedQuickRenderer(game, dest);
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