
namespace GensouGL.Graphics;

public sealed class ColorInversionEffect : PostProcessEffect
{
    public ColorInversionEffect()
    {
    }

    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        Shader.ColorInversion.Bind();

        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}