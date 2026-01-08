
namespace GensouGL.Graphics;

/// <summary>
/// A post-processing effect that adjusts the final color output using settings such as saturation, contrast, and color filters.
/// </summary>
public sealed class ColorGradingEffect : PostProcessEffect
{
    private ColorGradingSettings _settings;

    /// <summary>
    /// Gets the settings used to control the color grading parameters.
    /// </summary>
    public ColorGradingSettings Settings => _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorGradingEffect"/> with default settings.
    /// </summary>
    public ColorGradingEffect()
    {
        _settings = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorGradingEffect"/> with specific settings.
    /// </summary>
    /// <param name="settings">The color grading configuration to apply.</param>
    public ColorGradingEffect(ColorGradingSettings settings)
    {
        _settings = settings;
    }

    internal override void Apply(Renderer renderer, Screen source, Screen dest)
    {
        Shader shader = Shader.ColorGrading;
        shader.Bind();

        _settings.SetUniform(shader);

        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}