namespace GensouGL.Graphics;

using SixLabors.ImageSharp.Processing;
using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;

/// <summary>
/// Represents a page of font atlas array.
/// </summary>
public sealed class FontAtlasPage : IDisposable
{
    private static readonly int AtlasSize = 512;

    private Texture _texture;

    // context
    private int _cursorLeft = 0;
    private int _cursorTop = 0;
    private int _maxHeight = 0;

    private bool _disposed = false;

    /// <summary>
    /// Gets the underlying texture.
    /// </summary>
    public Texture Texture => _texture;

    /// <summary>
    /// Initializes a new instance of <see cref="FontAtlasPage"/>.
    /// </summary>
    public FontAtlasPage()
    {
        _texture = new Texture(AtlasSize, AtlasSize);
    }

    internal bool CanPack(IMS.Image<IMSP.Rgba32> image)
    {
        if (_cursorLeft + image.Width <= AtlasSize && _cursorTop + image.Height <= AtlasSize) return true;
        if (_cursorTop + image.Height > AtlasSize) return false;

        return _cursorTop + _maxHeight + image.Height <= AtlasSize && image.Width <= AtlasSize;
    }

    // Pack the given image and returns the uv coords.
    internal Rect PackUnchecked(IMS.Image<IMSP.Rgba32> image)
    {
        if (_cursorLeft + image.Width > AtlasSize)
        {
            // break line
            _cursorLeft = 0;
            _cursorTop += _maxHeight;
            _maxHeight = 0;
        }



        

        Rect imgRect = new Rect(_cursorLeft, _cursorTop, image.Width, image.Height);
        Rect uvRect = new Rect(imgRect.X / AtlasSize, imgRect.Y / AtlasSize, imgRect.Width / AtlasSize, imgRect.Height / AtlasSize);

        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);

        TextureService.PushAndBind(_texture.TextureId);

        GL.TexSubImage2D(TextureTarget.Texture2D, 0, _cursorLeft, _cursorTop, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        TextureService.Pop();





        _cursorLeft += image.Width;
        _maxHeight = int.Max(_maxHeight, image.Height);

        return uvRect;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _texture?.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="FontAtlasPage"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="FontAtlasPage"/> destructor.
    /// </summary>
    ~FontAtlasPage()
    {
        Dispose(false);
    }
}