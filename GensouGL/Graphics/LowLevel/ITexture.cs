namespace GensouGL.Graphics;

public interface ITexture
{
    public int TextureId { get; }
    public int Width { get; }
    public int Height { get; }
    public Rect Coords { get; }
}