namespace GensouGL.Graphics;

/// <summary>
/// A post-processing effect that inverts the colors of the screen (negative effect).
/// </summary>
public sealed class ColorInversionEffect : PostProcessEffect
{
    /// <summary>
    /// Gets the singleton instance of <see cref="ColorInversionEffect"/>.
    /// </summary>
    public static ColorInversionEffect Instance { get; } = new();

    private ColorInversionEffect()
    {
    }

    internal override void Apply(Renderer renderer, Screen source, Screen dest)
    {
        Shader.ColorInversion.Bind();

        FullscreenQuadRenderer.Draw(dest, source.Texture);
    }
}