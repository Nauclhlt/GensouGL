
namespace GensouGL.Graphics;

public sealed class TextureMaskEffect : PostProcessEffect
{
    private ITexture _mask;

    public ITexture Mask
    {
        get => _mask;
        set => _mask = value;
    }

    public TextureMaskEffect(ITexture mask)
    {
        _mask = mask;
    }

    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        Shader.TextureMask.Bind();
        Shader.TextureMask.SetUniformInt("pTexture", 0);
        Shader.TextureMask.SetUniformInt("pMask", 1);

        TextureService.PushAndBind(_mask.TextureId, 1);

        FullscreenQuadRenderer.Draw(dest, source.Texture);
        
        TextureService.Pop(1);
    }
}
