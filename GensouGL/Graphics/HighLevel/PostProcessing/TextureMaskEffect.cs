
namespace GensouGL.Graphics;

/// <summary>
/// A post-processing effect that applies a texture-based mask to the screen.
/// This can be used for vignettes, UI transitions, or complex visibility masking.
/// </summary>
public sealed class TextureMaskEffect : PostProcessEffect
{
    private ITexture _mask;

    /// <summary>
    /// Gets or sets the texture used as the alpha or visibility mask.
    /// </summary>
    public ITexture Mask
    {
        get => _mask;
        set => _mask = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureMaskEffect"/> with a specific mask texture.
    /// </summary>
    /// <param name="mask">The texture to use for masking.</param>
    public TextureMaskEffect(ITexture mask)
    {
        _mask = mask;
    }

    internal override void Apply(Renderer renderer, Screen source, Screen dest)
    {
        Shader.TextureMask.Bind();
        Shader.TextureMask.SetUniformInt("pTexture", 0);
        Shader.TextureMask.SetUniformInt("pMask", 1);

        TextureService.PushAndBind(_mask.TextureId, 1);

        FullscreenQuadRenderer.Draw(dest, source.Texture);
        
        TextureService.Pop(1);
    }
}
