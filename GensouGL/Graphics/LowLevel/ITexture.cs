namespace GensouGL.Graphics;

public interface ITexture
{
    public int TextureId { get; }
    public int BaseWidth { get; }
    public int BaseHeight { get; }
    public float Width { get; }
    public float Height { get; }
    public Rect Coords { get; }
}

public static class ITextureExtensions
{
    public static Subtexture GetSubtexture(this ITexture self, Rect rect)
    {
        return new Subtexture(self, new Rect(rect.X / self.Width, rect.Y / self.Height, rect.Width / self.Width, rect.Height / self.Height));
    }
}