namespace GensouGL.Graphics;

/// <summary>
/// Represents a base interface for textures.
/// </summary>
public interface ITexture
{
    /// <summary>
    /// Gets the texture id.
    /// </summary>
    public int TextureId { get; }
    /// <summary>
    /// Gets the width of the base texture.
    /// </summary>
    public int BaseWidth { get; }
    /// <summary>
    /// Gets the height of the base texture.
    /// </summary>
    public int BaseHeight { get; }
    /// <summary>
    /// Gets the width of the current texture.
    /// </summary>
    public float Width { get; }
    /// <summary>
    /// Gets the height of the current texture.
    /// </summary>
    public float Height { get; }
    /// <summary>
    /// Gets the UV coords.
    /// </summary>
    public Rect Coords { get; }
}

/// <summary>
/// <see cref="ITexture"/> extensions.
/// </summary>
public static class ITextureExtensions
{
    /// <summary>
    /// Gets the sub-texture representing the specified rectangular area of the source texture.
    /// </summary>
    /// <param name="self">Self.</param>
    /// <param name="rect">Source rectangle.</param>
    /// <returns>New <see cref="ITexture"/>.</returns>
    public static ITexture GetSubtexture(this ITexture self, Rect rect)
    {
        return new Subtexture(self, new Rect(rect.X / self.Width, rect.Y / self.Height, rect.Width / self.Width, rect.Height / self.Height));
    }

    /// <summary>
    /// Gets the vertically-flipped texture.
    /// </summary>
    /// <param name="self">Self.</param>
    /// <returns>Flipped texture.</returns>
    public static ITexture VFlipped(this ITexture self)
    {
        return new Subtexture(self, new Rect(self.Coords.X, self.Coords.Bottom, self.Coords.Width, -self.Coords.Height));
    }

    /// <summary>
    /// Gets the horizontally-flipped texture.
    /// </summary>
    /// <param name="self">Self.</param>
    /// <returns>Flipped texture.</returns>
    public static ITexture HFlipped(this ITexture self)
    {
        return new Subtexture(self, new Rect(self.Coords.Right, self.Coords.Y, -self.Coords.Width, self.Coords.Height));
    }
}