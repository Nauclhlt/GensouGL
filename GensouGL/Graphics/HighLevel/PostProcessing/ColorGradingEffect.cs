
namespace GensouGL.Graphics;

public  sealed class ColorGradingEffect : PostProcessEffect
{
    private ColorGradingSettings _settings;

    public ColorGradingSettings Settings => _settings;

    public ColorGradingEffect()
    {
        _settings = new();
    }

    public ColorGradingEffect(ColorGradingSettings settings)
    {
        _settings = settings;
    }

    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        Shader shader = Shader.ColorGrading;
        shader.Bind();

        _settings.SetUniform(shader);

        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}