
namespace GensouGL.Graphics;

public sealed class DoNothingEffect : PostProcessEffect
{
    internal override void Apply(Game game, Renderer renderer, Screen source, Screen dest)
    {
        QuickRenderer qr = GetSharedQuickRenderer(game, dest);
        qr.SetBlendMode(BlendMode.Normal);
        qr.DrawTexture(0, 540, 960, -540, source.Texture);
    }
}