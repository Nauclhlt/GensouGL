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
    public static ITexture GetSubtexture(this ITexture self, Rect rect)
    {
        return new Subtexture(self, new Rect(rect.X / self.Width, rect.Y / self.Height, rect.Width / self.Width, rect.Height / self.Height));
    }

    public static ITexture VFlipped(this ITexture self)
    {
        return new Subtexture(self, new Rect(self.Coords.X, self.Coords.Bottom, self.Coords.Width, -self.Coords.Height));
    }

    public static ITexture HFlipped(this ITexture self)
    {
        return new Subtexture(self, new Rect(self.Coords.Right, self.Coords.Y, -self.Coords.Width, self.Coords.Height));
    }
}